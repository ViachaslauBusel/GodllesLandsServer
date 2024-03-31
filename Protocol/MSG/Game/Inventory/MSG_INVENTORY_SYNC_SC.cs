using Protocol.Data.Items;
using Protocol.Data.Items.Network;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Inventory
{
    [MessagePack(Opcode.MSG_INVENTORY_SYNC, Channel.Queue)]
    public struct MSG_INVENTORY_SYNC_SC
    {
        public ItemStorageType Bag { get; set; }
        public int CurrentWeight { get; set; }
        public int MaxWeight { get; set; }
        public int CurrentItemsCount { get; set; }
        public int MaxItemsCount { get; set; }
        public List<ItemSyncData> Items { get; set; }
    }
}
