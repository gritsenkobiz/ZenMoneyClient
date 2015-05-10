using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public sealed class TransactionTag
    {
        //id => 5515174
        [JsonProperty("user")]
        public int User { get; set; }

        [JsonProperty("transaction")]
        public int Transaction { get; set; }

        [JsonProperty("tag_group")]
        public int TagGroup { get; set; }

        [JsonProperty("tag0")]
        public int Tag0 { get; set; }

        [JsonProperty("tag1")]
        public int Tag1 { get; set; }

        [JsonProperty("tag2")]
        public int Tag2 { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("changed")]
        public int Changed { get; set; }
    }
}