using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Commands
{
    public struct InteractionEndRequestCommand : ICommand
    {
        public int PlayerCharacterObjectID { get; set; }
    }
}
