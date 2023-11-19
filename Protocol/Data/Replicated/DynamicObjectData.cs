using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated
{
    public struct DynamicObjectData : IReplicationData
    {
        public byte Version { get; set; }
    }
}
