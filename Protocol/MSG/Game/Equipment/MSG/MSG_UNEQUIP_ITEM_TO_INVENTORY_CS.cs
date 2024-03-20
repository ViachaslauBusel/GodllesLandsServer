using Protocol.Data.Items;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Equipment.MSG
{
    [MessagePack(Opcode.MSG_UNEQUIP_ITEM_TO_INVENTORY, Channel.Reliable)]
    public struct MSG_UNEQUIP_ITEM_TO_INVENTORY_CS
    {
        public long ItemUID { get; set; }
        public ItemStorageType ToStorageType { get; set; }
        public int ToCellIndex { get; set; }
    }
}
