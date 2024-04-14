using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.MSG.Game.Test
{
    [MessageObject]
    public struct Point
    {
        public Vector3 Position { get; set;}
        public PointColor Color { get; set; }
    }
}
