using Game.Items;
using Game.Items.Components;
using NetworkGameEngine;
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
        private ItemStorageComponent _itemStorage;
        private Bag _primaryInventory;
        private Bag _secondaryInventory;

        public Bag PrimaryInventory => _primaryInventory;
        public Bag SecondaryInventory => _secondaryInventory;

        [Inject]
        public void InjectServices(ItemUniqueIdGenerator itemUniqueIdGenerator)
        {
           
        }

        public override void Init()
        {
            _itemStorage = GetComponent<ItemStorageComponent>();

            _primaryInventory = new Bag(_itemStorage, InventoryBagType.Primary, 20, 100);
            _secondaryInventory = new Bag(_itemStorage, InventoryBagType.Secondary, 40, 400);
        }

        public bool AddItem(Item item)
        {
            return _secondaryInventory.AddItem(item) || _primaryInventory.AddItem(item);
        }
    }
}
