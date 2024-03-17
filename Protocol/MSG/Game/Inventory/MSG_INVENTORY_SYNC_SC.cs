using Protocol.Data.Items.Network;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Inventory
{
    public enum InventoryBagType: byte
    {
        Primary,
        Secondary
    }
    [MessagePack(Opcode.MSG_INVENTORY_SYNC, Channel.Queue)]
    public struct MSG_INVENTORY_SYNC_SC
    {
        public InventoryBagType Bag { get; set; }
        public int CurrentWeight { get; set; }
        public int MaxWeight { get; set; }
        public int CurrentItemsCount { get; set; }
        public int MaxItemsCount { get; set; }
        public List<ItemSyncData> Items { get; set; }
    }
}
