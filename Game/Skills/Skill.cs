using DataFileProtocol.Skills;
using Game.Skills.Data;
using Game.Skills.Handler;
using NetworkGameEngine;

namespace Game.Skills
{
    public class Skill
    {
        private SkillData _data;
        private ISkillHandler _handler;

        public Skill(Component component, int skillID)
        {
            _data = SkillsDataStore.GetData(skillID);
            _handler = SkillsHandlerStore.CreateHandler(_data.skillType);
            _handler?.Init(component, _data);
        }

        public void Use(GameObject target)
        {
            _handler?.Use(target);
        }
    }
}
