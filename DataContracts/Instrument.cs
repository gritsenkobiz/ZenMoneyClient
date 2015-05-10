using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Instrument
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("shortTitle")]
        public string ShortTitle { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }
    }
}