using System;
using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class ReminderMarker
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public int User { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

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

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("notify")]
        public bool Notify { get; set; }

        [JsonProperty("payee")]
        public string Payee { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("merchant")]
        public Guid? Merchant { get; set; }

        [JsonProperty("tag")]
        public Guid[] Tags { get; set; }

    }
}