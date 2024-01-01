using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.ToServer
{
    [MessagePack(Opcode.MSG_UNIT_TARGET_REQUEST_CS, Channel.Discard)]
    public struct MSG_UNIT_TARGET_REQUEST_CS
    {
        public int GameObjectId { get; set;}
    }
}
