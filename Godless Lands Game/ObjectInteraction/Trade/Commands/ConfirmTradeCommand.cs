using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Commands
{
    public struct ConfirmTradeCommand : ICommand
    {
        public int RequesterId { get; set; }
    }
}
