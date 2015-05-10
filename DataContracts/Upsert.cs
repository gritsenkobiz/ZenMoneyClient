using System;

namespace ZeMoney.Common.DataContracts
{
    public class Upsert
    {
        public Guid Id { get; set; }

        public UpsertType Type { get; set; }
    }
}