using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("type")]
        public CategoryType Type { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}