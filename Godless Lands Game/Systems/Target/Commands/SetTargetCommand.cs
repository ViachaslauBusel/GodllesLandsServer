using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Target.Commands
{
    public struct SetTargetCommand : ICommand
    {
        public int GameObjectUnitID;
    }
}
