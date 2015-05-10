using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class ForeignFormat
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("regexp")]
        public string Regexp { get; set; }

        [JsonProperty("columns")]
        public string Columns { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("company")]
        public int Company { get; set; }

    }
}