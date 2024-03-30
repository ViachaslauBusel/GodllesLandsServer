using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Commands
{
    public struct InteractionStartConfirmation
    {
        public bool Result { get; internal set; }
        public List<Component> Components { get; internal set; }
    }
}
