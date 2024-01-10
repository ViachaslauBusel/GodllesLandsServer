using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Animation
{
    [MessageObject]
    public struct AnimationPlaybackBuffer : IReplicationData
    {
        public byte Version { get; set; }
        public List<AnimationData> PlaybackHistory { get; set; }
    }
}
