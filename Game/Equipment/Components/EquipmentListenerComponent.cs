using Game.Inventory.Components;
using Game.Items;
using Game.Items.Components;
using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.Equipment.MSG;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Equipment.Components
{
    public class EquipmentListenerComponent : Component
    {
        private ItemStorageComponent _itemStorage;
        private NetworkTransmissionComponent _networkTransmission;
        private EquipmentComponent _equipment;
        private InventoryComponent _inventory;

        public override void Init()
        {
            _itemStorage = GetComponent<ItemStorageComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _equipment = GetComponent<EquipmentComponent>();
            _inventory = GetComponent<InventoryComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_UNEQUIP_ITEM_TO_INVENTORY, UnequipItemToInventory);
            _networkTransmission.RegisterHandler(Opcode.MSG_DESTROY_ITEM_EQUIPMENT, DestroyItemEquipment);
        }

        private void DestroyItemEquipment(Packet packet)
        {
            packet.Read(out MSG_DESTROY_ITEM_EQUIPMENT_CS msg);
            Item item = _equipment.TakeItem(msg.ItemUID);
            if (item == null) return;
            _itemStorage.DestroyItem(item);
        }

        public void UnequipItemToInventory(Packet packet)
        {
            packet.Read(out MSG_UNEQUIP_ITEM_TO_INVENTORY_CS msg);
            var item = _equipment.TakeItem(msg.ItemUID);
            if (item == null)
            {
                return;
            }

            _inventory.AddItem(msg.ToStorageType, msg.ToCellIndex, item);
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_UNEQUIP_ITEM_TO_INVENTORY);
            _networkTransmission.UnregisterHandler(Opcode.MSG_DESTROY_ITEM_EQUIPMENT);
        }
    }
}
