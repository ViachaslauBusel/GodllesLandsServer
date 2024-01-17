using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Animation
{
    public struct AnimationStateData : IReplicationData
    {
        public byte Version { get; set; }
        public AnimationStateID AnimationStateID { get; set; }

    }
}
