using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public sealed class DiffObject
    {
        [JsonProperty("lastServerTimestamp")]    
        public long LastSyncronizationTimestamp { get; set; }

        [JsonProperty("currentClientTimestamp")]    
        public long ClientTimeStamp { get; set; }

        [JsonProperty("transaction")]
        public Transaction[] Transactions { get; set; }

    }
}