using Game.Equipment.Components;
using Game.Inventory.Components;
using Game.Items.ItemUsageLogic;
using Game.Items;
using Protocol.MSG.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkGameEngine;

namespace Godless_Lands_Game.Items.ItemUsageLogic
{
    public class PickaxeUsageLogic : IItemUsageLogic
    {
        private EquipmentComponent _equipment;
        private InventoryComponent _inventory;

        public void Init(Component component)
        {
            _equipment = component.GetComponent<EquipmentComponent>();
            _inventory = component.GetComponent<InventoryComponent>();
        }

        public bool Use(Item item)
        {
            Item equipedItem = _equipment.Take(EquipmentType.PickaxeTool);
            Item inventoryItem = _inventory.TakeItem(item.UniqueID);
            _equipment.Equip(EquipmentType.PickaxeTool, inventoryItem);
            _inventory.AddItem(equipedItem);
            return true;
        }
    }
}
