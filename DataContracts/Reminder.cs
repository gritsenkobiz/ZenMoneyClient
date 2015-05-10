using System;
using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Reminder
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public int User { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("income")]
        public double Income { get; set; }

        [JsonProperty("incomeAccount")]
        public Guid IncomeAccount { get; set; }

        [JsonProperty("incomeInstrument")]
        public int IncomeInstrument { get; set; }

        [JsonProperty("outcome")]
        public double Outcome { get; set; }

        [JsonProperty("outcomeAccount")]
        public Guid OutcomeAccount { get; set; }

        [JsonProperty("outcomeInstrument")]
        public int OutcomeInstrument { get; set; }

        [JsonProperty("tag")]
        public Guid[] Tags { get; set; }

        [JsonProperty("step")]
        public int Step { get; set; }

        [JsonProperty("notify")]
        public bool Notify { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("points")]
        public int[] Points { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("payee")]
        public string Payee { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("merchant")]
        public Guid? Merchant { get; set; }


    }
}