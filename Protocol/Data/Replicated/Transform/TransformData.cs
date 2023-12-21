using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.Data.Replicated.Transform
{


    public struct TransformData : IReplicationData
    {
        public byte Version { get; set; }
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public float Velocity { get; set; }
        public bool InMove { get; set; }
    }
}
