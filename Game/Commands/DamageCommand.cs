using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Commands
{
    public class DamageCommand : ICommand
    {
        public int fromObjID;
        public int PhysicalDamage;
    }
}
