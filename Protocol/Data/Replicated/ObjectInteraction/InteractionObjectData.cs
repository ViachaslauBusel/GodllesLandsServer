using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.ObjectInteraction
{
    public struct InteractionObjectData : IReplicationData
    {
        public bool IsInteractive { get; set; }
        public byte Version { get; set; }
    }
}
