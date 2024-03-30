using Game.DataSync;
using Game.Items;
using Game.Items.Components;
using NetworkGameEngine.Debugger;
using Protocol.MSG.Game.Inventory;

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
            if(item == null)
            {
                Debug.Log.Fatal($"Cannot put null item into cell:{_slotIndex}");
                return false;
            }
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

        internal Item TakeItem()
        {
            long uid = _uid;
            _uid = 0;
            SetDataSyncPending();
            return _itemStorage.GetItem(uid);
        }

        internal void RemoveItem(int count)
        {
           if (IsEmpty)
            {
                Debug.Log.Fatal($"Cannot remove item from empty cell:{_slotIndex}");
                return;
            }

            if (Item.Data.IsStackable && Item.Count > count)
            {
                Item.RemoveCount(count);
                SetDataSyncPendingOnlyWithClient();
                return;
            }

            _itemStorage.DestroyItem(Item);
            _uid = 0;
            SetDataSyncPending();
        }
    }
}
