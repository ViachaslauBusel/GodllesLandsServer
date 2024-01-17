using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated
{
    public struct UnitName : IReplicationData
    {
        public string Name { get; set; }
        public byte Version { get; set; }
    }
}
