using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Target.Commands
{
    public struct UnmarkUnitAsTargetedByCommand : ICommand
    {
        public int GameObjectUnitID;
    }
}
