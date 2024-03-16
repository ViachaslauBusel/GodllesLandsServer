using Game.DataSync;
using Game.Items;
using Game.Items.Components;
using NetworkGameEngine.Debugger;
using Protocol.MSG.Game.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Inventory
{
    public class Cell : ClientAndDbSyncElement
    {
        private long _uid;
        private InventoryBagType _bagId;
        private int _slotIndex;
        private ItemStorageComponent _itemStorage;

        public Item Item => _itemStorage.GetItem(_uid);
        public int SlotIndex => _slotIndex;
        public bool IsEmpty => _uid == 0;
        public InventoryBagType BagType => _bagId;

        public Cell(ItemStorageComponent itemStorage, InventoryBagType bagId, int slotIndex)
        {
            _itemStorage = itemStorage;
            _uid = 0;
            _bagId = bagId;
            _slotIndex = slotIndex;
        }

        public bool PutItem(Item item)
        {
            SetDataSyncPending();
            if (IsEmpty)
            {
                _uid = _itemStorage.AddItem(item);
                return true;
            }
            
            Item thisItem = Item;
            if (thisItem.Data.ID == item.Data.ID)
            {
                thisItem.AddCount(item.Count);
                _itemStorage.DestroyItem(item);
                return true;
            }

            Debug.Log.Fatal($"Cannot put item:{item.Data.ID} into cell:{_slotIndex} because it is not empty");
            return false;
        }
    }
}
