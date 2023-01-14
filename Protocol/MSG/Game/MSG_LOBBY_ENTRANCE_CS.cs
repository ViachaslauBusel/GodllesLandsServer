using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_LOBBY_ENTRANCE, Channel.Reliable)]
    public struct MSG_LOBBY_ENTRANCE_CS
    {
    }
}
