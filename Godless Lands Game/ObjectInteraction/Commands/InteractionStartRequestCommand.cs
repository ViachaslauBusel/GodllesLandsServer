using NetworkGameEngine;
using Protocol.MSG.Game.ObjectInteraction;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction.Commands
{
    /// <summary>
    /// This command send from the character to the interaction object
    /// </summary>
    public struct InteractionStartRequestCommand : ICommand
    {
        public GameObject PlayerCharacterObject { get; set; }
        public IPlayerNetworkProfile PlayerProfile { get; set; }
    }
}
