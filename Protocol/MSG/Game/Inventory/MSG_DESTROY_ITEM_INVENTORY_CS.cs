using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Inventory
{
    [MessagePack(Opcode.MSG_DESTROY_ITEM_INVENTORY, Channel.Reliable)]
    public struct MSG_DESTROY_ITEM_INVENTORY_CS 
    {
        public long ItemUID { get; set; }
        public int ItemCount { get; set; }
    }
}
