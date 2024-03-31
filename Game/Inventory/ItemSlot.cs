using Game.Items;
using Protocol.Data.Items;
using Protocol.MSG.Game.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Inventory
{
    internal struct ItemSlot
    {
        public readonly ItemStorageType BagType;
        public readonly int SlotIndex;
        public readonly Item Item;

        public ItemSlot(ItemStorageType bagType, int index, Item item)
        {
            BagType = bagType;
            SlotIndex = index;
            Item = item;
        }
    }
}
