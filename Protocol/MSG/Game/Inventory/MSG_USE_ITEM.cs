using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Inventory
{
    [MessagePack(Opcode.MSG_USE_ITEM, Channel.Reliable)]
    public struct MSG_USE_ITEM
    {
        public long ItemUID { get; set; }
    }
}
