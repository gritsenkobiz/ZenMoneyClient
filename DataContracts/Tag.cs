using System;
using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public class Tag
    {
        public static Tag Empty = new Tag(){Id = Guid.Empty, Title = "Без категории"};

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public int User { get; set; }

        [JsonProperty("chnaged")]
        public long Chnaged { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("showIncome")]
        public bool ShowIncome { get; set; }

        [JsonProperty("showOutcome")]
        public bool ShowOutcome { get; set; }

        [JsonProperty("parent")]
        public Guid? Parent { get; set; }
    }
}