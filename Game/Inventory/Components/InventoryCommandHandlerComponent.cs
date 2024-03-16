using Game.Inventory.Commands;
using Game.Items;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Inventory.Components
{
    public class InventoryCommandHandlerComponent : Component, IReactCommandWithResult<AddItemToInventoryCommand, List<Item>>
    {
        private InventoryComponent _inventory;

        public override void Init()
        {
            _inventory = GetComponent<InventoryComponent>();
        }

        public List<Item> ReactCommand(ref AddItemToInventoryCommand command)
        {
            // Get the list of items from the command
            List<Item> items = command.Items;

            // Try to add each item to the inventory. If the item cannot be added, it remains in the list.
            items.RemoveAll(item => _inventory.AddItem(item));

            // Return the list of items that could not be added to the inventory
            return items;
        }
    }
}
