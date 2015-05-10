using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Phone
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("company")]
        public int Company { get; set; }
    }
}