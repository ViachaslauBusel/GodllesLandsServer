using Game.Equipment.Components;
using Game.Inventory.Components;
using NetworkGameEngine;
using Protocol.MSG.Game.Equipment;

namespace Game.Items.ItemUsageLogic
{
    public class WeaponUsageLogic : IItemUsageLogic
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
            Item equipedItem = _equipment.Take(EquipmentType.WeaponRightHand);
            Item inventoryItem = _inventory.TakeItem(item.UniqueID);
            _equipment.Equip(EquipmentType.WeaponRightHand, inventoryItem);
            _inventory.AddItem(equipedItem);
            return true;
        }
    }
}
