using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Drop
{
    public struct LootableUnitData : IReplicationData
    {
        public byte Version { get; set; }
    }
}
