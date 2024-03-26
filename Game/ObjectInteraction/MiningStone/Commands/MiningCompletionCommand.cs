using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction.MiningStone.Commands
{
    public struct MiningCompletionCommand : ICommand
    {
        public GameObject CharacterObj;
    }
}
