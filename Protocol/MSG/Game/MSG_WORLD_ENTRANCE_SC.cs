using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{

    [MessagePack(Opcode.MSG_WORLD_ENTRANCE, Channel.Reliable)]
    public struct MSG_WORLD_ENTRANCE_SC
    {
    }
}
