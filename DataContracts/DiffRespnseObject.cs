using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZeMoney.Common.DataContracts
{
    [JsonObject]
    public sealed class DiffRespnseObject
    {
        [JsonProperty("serverTimestamp")]
        public long ServerTimestamp { get; set; }


        [JsonProperty("deletion")]
        public Deletion[] Deletions { get; set; }

        [JsonProperty("country")]
        public Country[] Countries { get; set; }

        [JsonProperty("company")]
        public Company[] Companies { get; set; }

        [JsonProperty("foreignFormat")]
        public ForeignFormat[] ForeignFormats { get; set; }

        [JsonProperty("phone")]
        public Phone[] Phones { get; set; }

        [JsonProperty("reminder")]
        public Reminder[] Reminders { get; set; }

        [JsonProperty("instrument")]
        public Instrument[] Instruments { get; set; }

        [JsonProperty("user")]
        public User[] Users { get; set; }

        [JsonProperty("account")]
        public Account[] Accounts { get; set; }

        [JsonProperty("reminderMarker")]
        public ReminderMarker[] ReminderMarkers { get; set; }

        [JsonProperty("transaction")]
        public Transaction[] Transactions { get; set; }

        [JsonProperty("tag")]
        public Tag[] Tags { get; set; }

        [JsonProperty("merchant")]
        public Merchant[] Merchants { get; set; }

    }
}