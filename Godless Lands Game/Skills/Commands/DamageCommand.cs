using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Skills.Commands
{
    public struct DamageCommand : ICommand
    {
        public GameObject Attacker;
        public int PAttack { get; internal set; }
    }
}
