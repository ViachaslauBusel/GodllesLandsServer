using Game.Inventory.Components;
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
        private NetworkTransmissionComponent _networkTransmission;
        private EquipmentComponent _equipment;
        private InventoryComponent _inventory;

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _equipment = GetComponent<EquipmentComponent>();
            _inventory = GetComponent<InventoryComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_UNEQUIP_ITEM_TO_INVENTORY, UnequipItemToInventory);
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
        }
    }
}
