using DataFileProtocol.Skills;
using NetworkGameEngine;

namespace Game.Skills.Handler
{
    public interface ISkillHandler
    {
        void Init(Component component, SkillData skill);
        void Use(GameObject target);
    }
}