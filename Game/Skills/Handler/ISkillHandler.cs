using DataFileProtocol.Skills;
using NetworkGameEngine;

namespace Game.Skills.Handler
{
    public interface ISkillHandler
    {
        bool InUse { get; }

        void Init(Component component, SkillData skill);
        bool PreProcessSkill(GameObject target);
        void PostProcessSkill();
    }
}