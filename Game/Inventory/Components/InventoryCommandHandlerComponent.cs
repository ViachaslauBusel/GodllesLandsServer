using Game.Inventory.Commands;
using Game.Items;
using Game.Messenger;
using NetworkGameEngine;
using Protocol.MSG.Game.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Inventory.Components
{
    public class InventoryCommandHandlerComponent : Component, IReactCommandWithResult<AddItemToInventoryCommand, List<Item>>, IReactCommand<AddItemToInventoryCommandNoRet>
    {
        private InventoryComponent _inventory;
        private MessageBroadcastComponent _messageBroadcast;

        public override void Init()
        {
            _inventory = GetComponent<InventoryComponent>();
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();
        }

        public List<Item> ReactCommand(ref AddItemToInventoryCommand command)
        {
            // Get the list of items from the command
            List<Item> items = command.Items;

            // Try to add each item to the inventory. If the item cannot be added, it remains in the list.
            items.RemoveAll(item => AddItemToInventory(item));

            // Return the list of items that could not be added to the inventory
            return items;
        }

        public void ReactCommand(ref AddItemToInventoryCommandNoRet command)
        {
            command.Items.RemoveAll(item => AddItemToInventory(item));
        }

        private bool AddItemToInventory(Item item)
        {
            if(_inventory.AddItem(item))
            {
                _messageBroadcast.SendMessage(MsgLayer.System, $"Added {item.Data.ID} to inventory");
                return true;
            }
            else
            {
                _messageBroadcast.SendMessage(MsgLayer.System, $"Inventory is full. Cannot add {item.Data.ID}");
                return false;
            }
        }
    }
}
