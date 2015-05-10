using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using AsyncOAuth;
using Gritsenko;
using Gritsenko.Universal.Common;
using Gritsenko.Universal.Extensions;
using Lex.Db;
using Newtonsoft.Json;
using ZeMoney.Common.DataContracts;
using Account = ZeMoney.Common.DataContracts.Account;

namespace ZeMoney.Common
{
    public sealed class ZenmoneyClient
    {
        private const string ApiBaseUrl = "http://api.zenmoney.ru";

        private const string ConsumerKey = "g36ed52c177504c9a62dd159abc9fd";
        private const string ConsumerSecret = "2d8df99796";

        private AccessToken _accessToken;
        private static ZenmoneyClient _instance;
        public static ZenmoneyClient Instance
        {
            get { return _instance ?? (_instance = new ZenmoneyClient()); }
        }

        public string LastErrorMessage { get; set; }

        public string StatusMessage { get; private set; }

        public bool IsAuthorized { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public Account[] Accounts
        {
            get { return _db.LoadAll<Account>(); }
        }

        public User User
        {
            get { return _db.LoadAll<User>().FirstOrDefault(); }
        }

        public Category[] Categorries { get; set; }
        
        public double Balance { get; set; }

        public Tag[] Tags
        {
            get { return _db.LoadAll<Tag>(); }
        }
        public Transaction[] Transactions { get { return GetTransactions(40); }}

        public Merchant[] Merchants
        {
            get { return _db.LoadAll<Merchant>(); }
        }

        public List<Upsert> Upserts { get; set; }

        /**********************************************************************************/
        // Functions
        /**********************************************************************************/
        public ZenmoneyClient()
        {
            Upserts = new List<Upsert>();
            IsAuthorized = false;
            InitLocalStorage();

            LoadState();

            OAuthUtility.ComputeHash = (keyBytes, bufferBytes) =>
            {
                var crypt = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
                var buffer = CryptographicBuffer.CreateFromByteArray(bufferBytes);
                var keyBuffer = CryptographicBuffer.CreateFromByteArray(keyBytes);
                var key = crypt.CreateKey(keyBuffer);
                var sigBuffer = CryptographicEngine.Sign(key, buffer);
                return sigBuffer.ToArray();
            };
        }

        /**********************************************************************************/

        public async Task<bool> Authorize()
        {
            Logger.Trace("Authorizing");

            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                StatusMessage = "Nothing";
                LastErrorMessage = "Password or login are no set.";
                return false;
            }

            IsAuthorized = false;

            var client = new HttpClient();

            try
            {
                var authorizer = new OAuthAuthorizer(ConsumerKey, ConsumerSecret);
                var tokenResponse = await authorizer.GetRequestToken("http://api.zenmoney.ru/oauth/request_token");
                var requestToken = tokenResponse.Token;

                var pinRequestUrl = authorizer.BuildAuthorizeUrl("http://api.zenmoney.ru/access/", requestToken) + "&mobile";

                var authPage = await client.GetStringAsync(pinRequestUrl);

                var query = new FormUrlEncodedContent(new Collection<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("oauth_callback", "http://grtisenko.biz/zenmoney"),
                        new KeyValuePair<string, string>("login", Login),
                        new KeyValuePair<string, string>("password", Password)
                    });

                var request = await client.PostAsync(pinRequestUrl, query);

                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var responseStr = await request.Content.ReadAsStringAsync();
                    //LastErrorMessage = responseStr;
                    throw new UnauthorizedAccessException("Json content: " + responseStr);
                }

                var args = HttpUtil.ParseQueryString(request.RequestMessage.RequestUri.ToString());

                var verificationCode = Uri.UnescapeDataString(args["oauth_verifier"]);

                var accessTokenResponse = await authorizer.GetAccessToken("http://api.zenmoney.ru/oauth/access_token", requestToken, verificationCode);

                _accessToken = accessTokenResponse.Token;

                IsAuthorized = true;
            }
            catch (HttpRequestException httpEx)
            {
                LastErrorMessage = "Ошибка подключения к серверу: " + httpEx.Message;
                Logger.LogException<ZenmoneyClient>(httpEx, "Authorization faild, due http request exception");                
            }
            catch (Exception ex)
            {
                LastErrorMessage = "Неправильное имя пользователя или пароль";
                Logger.LogException<ZenmoneyClient>(ex, "Authorization faild");
            }
            finally
            {
                client.Dispose();
            }
            return IsAuthorized;
        }

        /**********************************************************************************/

        public async void SaveTransaction(Transaction transaction)
        {
            if (transaction.User == 0)
            {
                transaction.User = User.Id;
            }
            transaction.Changed = DateTime.Now.ToTimeStamp();
            if (transaction.Created == 0)
            {
                transaction.Created = transaction.Changed;
            }
#if DEBUG
            Logger.Trace("Saving transaction: \n\n" + JsonConvert.SerializeObject(transaction));
#endif

            if (transaction.IncomeAccount == Guid.Empty ||  
                transaction.OutcomeAccount == Guid.Empty ||
                (Math.Abs(transaction.Outcome) < 0.01 && Math.Abs(transaction.Income) < 0.01))
            {
                Logger.Log("Transaction is not valid!");
                return;
            }
            var upsert = new Upsert() {Id = transaction.Id, Type = UpsertType.Transaction};
            Upserts.Add(upsert);

            _db.Table<Transaction>().Save(transaction);
            _db.Table<Upsert>().Save(upsert);
            
            await Syncronize();
        }

        /**********************************************************************************/

        public void RemoveeTransaction(Guid id)
        {
        }

        /**********************************************************************************/

        public async Task Syncronize()
        {
            try
            {
                var authResult = await Authorize();

                if (!authResult)
                {
                    Logger.Log("Can't login to ZenMoney server");
                    return;
                }

                await Diff();

                LoadState();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Exception in (ZenmoneyClient.cs)\\[ZenmoneyClient.Syncronize] ");
                throw;
            }
        }

        /**********************************************************************************/

        public async Task Diff(DiffObject diffObject = null)
        {
            var now = DateTime.Now;
            try
            {
                Logger.Trace("Diffiniarizing...");

                if (diffObject == null)
                {
                    diffObject = new DiffObject();
                }

                diffObject.LastSyncronizationTimestamp = _settings.LastServerTimestamp;
                diffObject.ClientTimeStamp = now.ToTimeStamp();

                diffObject.Transactions = GetPendingTransactions();

                var uri = new Uri(ApiBaseUrl + "/v6/diff/");

                var httpClient = OAuthUtility.CreateOAuthClient(ConsumerKey, ConsumerSecret, _accessToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var postJson = JsonConvert.SerializeObject(diffObject);

                Logger.Trace("Request: \n\n"+postJson);

                var response = await httpClient.PostAsync(uri, new StringContent(postJson, Encoding.UTF8, "application/json"));

                var json = await response.Content.ReadAsStringAsync();

                Logger.Trace("Response: \n\n" + json);

                var diffResponse = JsonConvert.DeserializeObject<DiffRespnseObject>(json);
                _settings.LastSyncTime = now;
                _settings.LastServerTimestamp = diffResponse.ServerTimestamp;

                await SaveChanges(diffResponse);


                if (diffResponse.Transactions != null)
                {
                    var updatetedTransactionIds = diffResponse.Transactions.Select(x => x.Id).ToArray();
                    Upserts.RemoveAll(x => updatetedTransactionIds.Contains(x.Id));
                    _db.Table<Upsert>().DeleteByKeys(updatetedTransactionIds);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Exception in (ZenmoneyClient.cs)\\[ZenmoneyClient.GetProfile] ");
                throw;
            }
        }

        /**********************************************************************************/

        private Transaction[] GetPendingTransactions()
        {
            var ids = Upserts
                .Where(x => x.Type == UpsertType.Transaction)
                .Select(x=>x.Id).ToArray();

            if (!ids.Any())
            {
                return new Transaction[0];
            }

            var transactions = _db.Table<Transaction>().LoadByKeys(ids);

            Logger.Trace("There are {0} pending transactions", ids.Count());

            return transactions.ToArray();
        }

        /**********************************************************************************/

        public Transaction[] GetTransactions(int count)
        {
            var result = new Transaction[0];
            try
            {
                count = Math.Min(_db.Count<Transaction>(), count);
                if (count == 0) return result;

                result = _db.LoadAll<Transaction>()
                    .OrderByDescending(x=>x.Date)
                    .Take(count)
                    .ToArray();

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Exception in (ZenmoneyClient.cs)\\[ZenmoneyClient.LoadState] ");
                throw;
            }
        }

        /**********************************************************************************/

        private string DbName = "ZenMoneyCache";
        private DbInstance _db;
        private ZenMoneySettings _settings;

        private void InitLocalStorage()
        {
            _db = new DbInstance(DbName);
            _db.Map<ZenMoneySettings>().Automap(x => x.Id);
            _db.Map<Account>().Automap(x => x.Id);
            _db.Map<Reminder>().Automap(x => x.Id);
            _db.Map<ReminderMarker>().Automap(x => x.Id);
            _db.Map<Instrument>().Automap(x => x.Id);
            _db.Map<Phone>().Automap(x => x.Id);
            _db.Map<User>().Automap(x => x.Id);
            _db.Map<ForeignFormat>().Automap(x => x.Id);
            _db.Map<Country>().Automap(x => x.Id);
            _db.Map<Company>().Automap(x => x.Id);
            _db.Map<Tag>().Automap(x => x.Id);
            _db.Map<Deletion>().Automap(x => x.Id);
            _db.Map<Merchant>().Automap(x => x.Id);

            _db.Map<Transaction>().Automap(x => x.Id);
            _db.Map<TransactionIndex>().Automap(x => x.Index);
            _db.Map<Upsert>().Automap(x => x.Id);
            
            _db.Initialize();
        }

        /**********************************************************************************/

        public async Task SaveChanges(DiffRespnseObject diffRespnse)
        {
            try
            {
                await SaveTable(new[] {_settings});
                await SaveTable(diffRespnse.Accounts);
                await SaveTable(diffRespnse.Reminders);
                await SaveTable(diffRespnse.ReminderMarkers);
                await SaveTable(diffRespnse.Instruments);
                await SaveTable(diffRespnse.Phones);
                await SaveTable(diffRespnse.Users);
                await SaveTable(diffRespnse.ForeignFormats);
                await SaveTable(diffRespnse.Countries);
                await SaveTable(diffRespnse.Companies);
                await SaveTable(diffRespnse.Tags);
                await SaveTable(diffRespnse.Deletions);
                await SaveTable(diffRespnse.Merchants);
                await SaveTable(diffRespnse.Transactions);
                await SaveTable(Upserts);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Exception in (ZenmoneyClient.cs)\\[ZenmoneyClient.SaveChanges] ");
            }
        }


        /**********************************************************************************/

        private async Task SaveTable<T>(IEnumerable<T> table) where T : class
        {
            if (table != null)
                await _db.Table<T>().SaveAsync(table);
        }

        /**********************************************************************************/

        private void LoadState()
        {
            try
            {
                _settings = _db.LoadByKey<ZenMoneySettings>(0);

                Upserts = _db.LoadAll<Upsert>().ToList();

                if (Accounts != null)
                {
                    Balance = Accounts.Where(x => x.InBalance).Sum(x => x.Balance);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Exception in (ZenmoneyClient.cs)\\[ZenmoneyClient.LoadState] ");
            }

            if (_settings == null)
            {
                _settings = new ZenMoneySettings();
            }
        }

    }
}
