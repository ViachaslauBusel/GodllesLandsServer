using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_PLAYER_INPUT_CS, RUCP.Channel.Discard)]
    public struct MSG_PLAYER_INPUT_CS
    {
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public float Velocity { get; set; }
        public bool InMove { get; set; }
    }
}
