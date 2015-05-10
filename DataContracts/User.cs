using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("country")]
        public int Country { get; set; }


        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

    }
}