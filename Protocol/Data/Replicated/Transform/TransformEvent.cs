using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.Data.Replicated.Transform
{
    public struct TransformEvent : IEvent
    {
        public Vector3 Position { get; set; }
        public MoveFlag MoveFlag { get; set; }
        public byte EventHappenedAtVersion { get; set; }
    }
}
