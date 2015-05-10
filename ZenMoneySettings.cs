using System;

namespace ZeMoney.Common
{
    public class ZenMoneySettings
    {
        public int Id { get; set; }

        public DateTime LastSyncTime { get; set; }
        public long LastServerTimestamp { get; set; }
    }
}