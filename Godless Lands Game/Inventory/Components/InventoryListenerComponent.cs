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
            _networkTransmission.RegisterHandler(Opcode.MSG_TRANSFER_ITEM_TO_ANOTHER_BAG, TransferItemToAnotherBag);
            _networkTransmission.RegisterHandler(Opcode.MSG_DESTROY_ITEM_INVENTORY, DestroyItemInventory);
        }

        private void DestroyItemInventory(Packet packet)
        {
            packet.Read(out MSG_DESTROY_ITEM_INVENTORY_CS destroy_item_inventory);

            if(destroy_item_inventory.ItemCount <= 0)
            {
                Debug.Log.Warn($"Item count is less than 0");
                return;
            }
            _inventory.RemoveItem(destroy_item_inventory.ItemUID, destroy_item_inventory.ItemCount);
        }

        private void TransferItemToAnotherBag(Packet packet)
        {
            packet.Read(out MSG_TRANSFER_ITEM_TO_ANOTHER_BAG transfer_item_to_another_bag);

            Bag targetBag = _inventory.PrimaryInventory.HasItem(transfer_item_to_another_bag.ItemUID)
                          ? _inventory.SecondaryInventory : _inventory.PrimaryInventory;

            Item item = _inventory.TakeItem(transfer_item_to_another_bag.ItemUID);

            targetBag.AddItem(item);
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
                Debug.Log.Warn($"Item with UID {use_item.ItemUID} can't be used");
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
            _networkTransmission.UnregisterHandler(Opcode.MSG_TRANSFER_ITEM_TO_ANOTHER_BAG);
            _networkTransmission.UnregisterHandler(Opcode.MSG_DESTROY_ITEM_INVENTORY);
        }
    }
}
