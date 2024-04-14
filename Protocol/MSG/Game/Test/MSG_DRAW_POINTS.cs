using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Test
{
    [MessagePack(Opcode.MSG_DRAW_POINTS, Channel.Reliable)]
    public struct MSG_DRAW_POINTS
    {
        public List<Point> Points { get; set; }
    }
}
