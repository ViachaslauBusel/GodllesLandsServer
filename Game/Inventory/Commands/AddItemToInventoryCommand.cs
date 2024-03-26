using Game.Items;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Inventory.Commands
{
    public struct AddItemToInventoryCommand : ICommand
    {
        public List<Item> Items;
    }
    public struct AddItemToInventoryCommandNoRet : ICommand
    {
        public List<Item> Items;
    }
}
