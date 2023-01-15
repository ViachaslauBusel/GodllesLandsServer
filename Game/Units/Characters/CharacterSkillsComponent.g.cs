using Game.Commands;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Units.Characters
{
    internal partial class CharacterSkillsComponent
    {
        public override void CallReact(ICommand cmd)
        {
           if(cmd is UseSkillComand) React(cmd as UseSkillComand);
        }
    }
}
