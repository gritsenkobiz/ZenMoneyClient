using System;
using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Merchant
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public int? User { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }
    }
}