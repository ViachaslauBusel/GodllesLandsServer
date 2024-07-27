using Game.Inventory.Components;
using Game.Systems.Stats.Components;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Items;
using Protocol.Data.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items.ItemUsageLogic
{
    internal class RestorePointUsageLogic : IItemUsageLogic
    {
        private BodyComponent _bodyComponent;
        private InventoryComponent _inventoryComponent;

        public void Init(Component component)
        {
            _bodyComponent = component.GetComponent<BodyComponent>();
            _inventoryComponent = component.GetComponent<InventoryComponent>();
        }

        public bool Use(Item item)
        {
            ElixirItemInfo data = (ElixirItemInfo)item.Data;

            if (data == null)
            {
                Debug.Log.Error($"Item {item.UniqueID} has wrong data type");
                return false;
            }

            if(_inventoryComponent.RemoveItem(item.UniqueID, 1) == false)
            {
                Debug.Log.Error($"Item {item.UniqueID} not found in inventory");
                return false;
            }

            _bodyComponent.Heal(data.RestoreHP, StatCode.HP, StatCode.MaxHP);
            return true;
        }
    }
}
