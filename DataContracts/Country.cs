using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Country
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

    }
}