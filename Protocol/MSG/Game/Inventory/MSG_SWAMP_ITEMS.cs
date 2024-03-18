using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Inventory
{
    [MessagePack(Opcode.MSG_SWAMP_ITEMS, Channel.Reliable)]
    public struct MSG_SWAMP_ITEMS
    {
        public long ItemUID { get; set; }
        public int ToCellIndex { get; set; }
    }
}
