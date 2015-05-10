using System;
using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Account
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("instrument")]
        public int Instrument { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("user")]
        public int User { get; set; }

        [JsonProperty("archive")]
        public bool Archive { get; set; }

        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("startBalance")]
        public double StartBalance { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("company")]
        public int? Company { get; set; }

        [JsonProperty("creditLimit")]
        public double CreditLimit { get; set; }
        
        [JsonProperty("enableSMS")]
        public bool EnableSms { get; set; }

        [JsonProperty("enableCorrection")]
        public bool EnableCorrection { get; set; }

        [JsonProperty("inBalance")]
        public bool InBalance { get; set; }
        
        [JsonProperty("syncID")]
        public string[] SyncID { get; set; }
    }
}