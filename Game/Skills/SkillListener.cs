using Game.Commands;
using NetworkGameEngine;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Skills
{
  
    internal static class SkillListener
    {
        //Handler
        internal static void UseSkill(Profile profile, Packet packet)
        {
            int skillID = 1;

            if(World.TryGetGameObject(profile.CharacterObjectID, out var character))
            {
                character.SendCommand(new UseSkillComand() { skillID = skillID });
            }
        }
    }
}
