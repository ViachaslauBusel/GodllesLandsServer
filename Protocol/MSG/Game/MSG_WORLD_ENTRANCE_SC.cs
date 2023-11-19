using RUCP;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.MSG.Game
{

    [MessagePack(Opcode.MSG_WORLD_ENTRANCE, Channel.Reliable)]
    public struct MSG_WORLD_ENTRANCE_SC
    {
        public Vector3 EntryPoint { get; set; }
    }
}

