using DataFileProtocol.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Skills.Handler
{
    public static class SkillsHandlerStore
    {

        public static ISkillHandler CreateHandler(SkillType skillType)
        {
            switch (skillType)
            {
                case SkillType.Melee:
                    return new MelleSkillHandler();
                case SkillType.Teleport:
                    return new TeleportSkillHandler();
                default:
                    return null;
            }
        }
    }
}
