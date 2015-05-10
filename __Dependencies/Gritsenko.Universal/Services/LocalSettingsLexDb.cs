using System.Threading.Tasks;
using Gritsenko.Universal.Abstract;
using Lex.Db;
using Newtonsoft.Json;

namespace Gritsenko.Universal.Services
{
    public class LocalSettingsLexDbService : ISettingsService
    {
        class Setting
        {
            public string Key;
            public string Value;
        }

        private string DbName = "lexDb";
        private DbInstance _db;

        private DbInstance Db
        {
            get { return _db ?? (_db = GetNewDbContext()); }
        }

        private DbInstance GetNewDbContext()
        {
            var db = new DbInstance(DbName);

            db.Map<Setting>().Automap(x => x.Key);
            db.Initialize();

            return db;
        }

        public TSetting Load<TSetting>(string key)
        {
            var setting = Db.Table<Setting>().LoadByKey(key);

            if (setting == null)
            {
                return default (TSetting);
            }

            var result = JsonConvert.DeserializeObject<TSetting>(setting.Value);

            return result;
        }

        public async Task Save(string key, object value)
        {
            await Db.Table<Setting>().SaveAsync(new Setting() { Key = key, Value = JsonConvert.SerializeObject(value) });
        }

    }
}