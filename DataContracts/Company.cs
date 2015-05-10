using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Company
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("www")]
        public string Www { get; set; }

        [JsonProperty("country")]
        public int? Country { get; set; }

        [JsonProperty("fullTitle")]
        public string FullTitle { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }
    }
}