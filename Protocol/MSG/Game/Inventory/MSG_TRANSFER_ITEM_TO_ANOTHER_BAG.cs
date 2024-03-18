using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Inventory
{
    [MessagePack(Opcode.MSG_TRANSFER_ITEM_TO_ANOTHER_BAG, Channel.Reliable)]
    public struct MSG_TRANSFER_ITEM_TO_ANOTHER_BAG
    {
        public long ItemUID { get; set; }
    }
}
