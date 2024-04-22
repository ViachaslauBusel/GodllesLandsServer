using Game.Items;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Commands
{
    public struct AddItemToTradeStorageCommand : ICommand
    {
        public int RequesterId { get; set; }
        public Item Item { get; set; }
        public int ToSlot { get; set; }
    }
}
