using System;
using Gritsenko.Universal.Extensions;
using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Transaction
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("income")]
        public double Income { get; set; }

        [JsonProperty("incomeAccount")]
        public Guid IncomeAccount { get; set; }

        [JsonProperty("incomeInstrument")]
        public int IncomeInstrument { get; set; }

        [JsonProperty("incomeBankID")]
        public string IncomeBankId { get; set; }

        [JsonProperty("outcome")]
        public double Outcome { get; set; }

        [JsonProperty("outcomeAccount")]
        public Guid OutcomeAccount { get; set; }

        [JsonProperty("outcomeInstrument")]
        public int OutcomeInstrument { get; set; }

        [JsonProperty("outcomeBankID")]
        public string OutcomeBankId { get; set; }

        [JsonProperty("latitude")]
        public double? Lattitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longtitude { get; set; }

        [JsonProperty("opIncome")]
        public int? OpIncome { get; set; }

        [JsonProperty("opIncomeInstrument")]
        public int? OpIncomeInstrument { get; set; }

        [JsonProperty("opOutcome")]
        public int? OpOutcome { get; set; }

        [JsonProperty("opOutcomeInstrument")]
        public int? OpOutcomeInstrument { get; set; }

        [JsonProperty("payee")]
        public string Payee { get; set; }

        [JsonProperty("tag")]
        public Guid[] Tags { get; set; }

        [JsonProperty("merchant")]
        public Guid? Merchant { get; set; }

        [JsonProperty("reminderMarker")]
        public Guid? ReminderMarker { get; set; }

        [JsonProperty("user")]
        public int User { get; set; }

        [JsonIgnore]
        public DateTime DateProxy
        {
            get
            {
                DateTime result;
                if (DateTime.TryParse(Date, out result))
                {
                    return result;
                }

                return default(DateTime);
            }
            set { Date = value.ToString("yyyy-MM-dd"); }
        }

        public static Transaction CreateNew()
        {
            return new Transaction()
            {
                Id=Guid.NewGuid(),
                Changed = DateTime.Now.ToTimeStamp()
            };
        }
    }
}
