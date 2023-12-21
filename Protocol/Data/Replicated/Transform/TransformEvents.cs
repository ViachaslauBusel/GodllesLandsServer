using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Transform
{
    public struct TransformEvents : IReplicationData
    {
        public byte Version {get; set; }
        public List<TransformEvent> Events { get; set; }
    }
}
