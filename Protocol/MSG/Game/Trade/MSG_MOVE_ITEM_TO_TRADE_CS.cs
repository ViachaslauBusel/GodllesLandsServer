using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Trade
{
    [MessagePack(Opcode.MSG_MOVE_ITEM_TO_TRADE, Channel.Reliable)]
    public struct MSG_MOVE_ITEM_TO_TRADE_CS
    {
        public long ItemUID { get; set; }
        public short Count { get; set; }
        public byte ToSlot { get; set; }
    }
}
