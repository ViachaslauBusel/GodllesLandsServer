using Game.Items;
using Game.Items.Components;
using Godless_Lands_Game.Inventory;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Items;
using Protocol.MSG.Game.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.Inventory.Components
{
    public class InventoryComponent : Component
    {
        private ItemsFactory _itemsFactory;
        private ItemStorageComponent _itemStorage;
        private Bag _primaryInventory;
        private Bag _secondaryInventory;

        public Bag PrimaryInventory => _primaryInventory;
        public Bag SecondaryInventory => _secondaryInventory;

        [Inject]
        public void InjectServices(ItemsFactory itemsFactory)
        {
            _itemsFactory = itemsFactory;
        }

        public override void Init()
        {
            _itemStorage = GetComponent<ItemStorageComponent>();

            _primaryInventory = new Bag(_itemStorage, _itemsFactory, ItemStorageType.PrimaryBag, 20, 5_000);
            _secondaryInventory = new Bag(_itemStorage, _itemsFactory, ItemStorageType.SecondaryBag, 40, 10_000);
        }

        public bool AddItem(Item item)
        {
            return _secondaryInventory.AddItem(item) || _primaryInventory.AddItem(item);
        }

        internal bool AddItem(ItemStorageType toStorageType, int toCellIndex, Item item)
        {
            Bag bag = toStorageType == ItemStorageType.PrimaryBag ? _primaryInventory : _secondaryInventory;

            if(bag.AddItem(item, toCellIndex))
            {
                return true;
            }

            return AddItem(item);
        }

        /// <summary>
        /// This method is used to get item from both primary and secondary inventories
        /// It's dont remove item from inventory or storage
        /// </summary>
        /// <param name="itemUID"></param>
        /// <returns></returns>
        internal Item GetItem(long itemUID)
        {
            if(_primaryInventory.HasItem(itemUID))
            {
                return _primaryInventory.GetItem(itemUID);
            }
            
            if(_secondaryInventory.HasItem(itemUID))
            {
                return _secondaryInventory.GetItem(itemUID);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get item from both primary and secondary inventories
        /// It's remove item from inventory but not from storage
        /// </summary>
        /// <param name="itemUID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal Item TakeItem(long itemUID)
        {
            if(_primaryInventory.HasItem(itemUID))
            {
                return _primaryInventory.TakeItem(itemUID);
            }
            else if(_secondaryInventory.HasItem(itemUID))
            {
                return _secondaryInventory.TakeItem(itemUID);
            }

            return null;
        }

        /// <summary>
        /// This method is used to get item from both primary and secondary inventories
        /// It's remove item from inventory but not from storage
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal List<ItemSlot> TakeItemByItemId(int itemID, int amount)
        {
            List<ItemSlot> takedItems = new List<ItemSlot>();
            _primaryInventory.TakeItemByItemId(itemID, amount, in takedItems);

            if (takedItems.Sum(i => i.Item.Count) >= amount) return takedItems;

            int countFromPrimary = takedItems.Count;

            _secondaryInventory.TakeItemByItemId(itemID, amount, in takedItems);

            if (takedItems.Sum(i => i.Item.Count) >= amount) return takedItems;

            // If items are not enough, return items to inventory
            for (int i = 0; i < takedItems.Count; i++)
            {
               AddItem(takedItems[i].BagType, takedItems[i].SlotIndex, takedItems[i].Item);
            }

            return null;
        }

        internal bool RemoveItem(long itemUID, int count)
        {
            if(_primaryInventory.HasItem(itemUID))
            {
               return _primaryInventory.RemoveItem(itemUID, count);
            }
            else if(_secondaryInventory.HasItem(itemUID))
            {
              return  _secondaryInventory.RemoveItem(itemUID, count);
            }

            return false;
        }

        internal void SwampItems(long itemUID, int toCellIndex)
        {
            if (_primaryInventory.HasItem(itemUID))
            {
                _primaryInventory.SwampItems(itemUID, toCellIndex);
            }
            else if (_secondaryInventory.HasItem(itemUID))
            {
                _secondaryInventory.SwampItems(itemUID, toCellIndex);
            }
        }

        internal bool ContainsItem(int itemId, int amount)
        {
            int count = _primaryInventory.GetItemCount(itemId) + _secondaryInventory.GetItemCount(itemId);
            return count >= amount;
        }
    }
}
