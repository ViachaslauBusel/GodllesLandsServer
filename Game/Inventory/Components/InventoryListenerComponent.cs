using Game.Items;
using Game.Items.Components;
using Game.NetworkTransmission;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.MSG.Game.Inventory;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Inventory.Components
{
    /// <summary>
    /// Component that listens network commands from the client
    /// </summary>
    public class InventoryListenerComponent : Component
    {
        private NetworkTransmissionComponent _networkTransmission;
        private InventoryComponent _inventory;
        private ItemUsageComponent _itemUsage;

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _inventory = GetComponent<InventoryComponent>();
            _itemUsage = GetComponent<ItemUsageComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_USE_ITEM, UseItem);
            _networkTransmission.RegisterHandler(Opcode.MSG_SWAMP_ITEMS, SwampItems);
        }

        private void SwampItems(Packet packet)
        {
            packet.Read(out MSG_SWAMP_ITEMS swamp_items);

            _inventory.SwampItems(swamp_items.ItemUID, swamp_items.ToCellIndex);
        }

        private void UseItem(Packet packet)
        {
            packet.Read(out MSG_USE_ITEM use_item);

            Item item = _inventory.GetItem(use_item.ItemUID);

            if (item == null)
            {
               Debug.Log.Error($"Item with UID {use_item.ItemUID} not found in inventory");
                return;
            }

            // Сheck if the item can be used
            if (_itemUsage.IsUsableItem(item) == false)
            {
                Debug.Log.Error($"Item with UID {use_item.ItemUID} can't be used");
                return;
            }

            if (_inventory.RemoveItem(item, 1) == false)
            {
                Debug.Log.Error($"Item with UID {use_item.ItemUID} can't be removed from inventory");
                return;
            }

            if (_itemUsage.Use(item) == false)
            {
                Debug.Log.Error($"Item with UID {use_item.ItemUID} can't be used");
                return;
            }
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_USE_ITEM);
            _networkTransmission.UnregisterHandler(Opcode.MSG_SWAMP_ITEMS);
        }
    }
}
