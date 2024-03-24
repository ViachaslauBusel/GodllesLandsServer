using DataFileProtocol.Skills;
using Game.Skills.Data;
using Game.Skills.Handler;
using NetworkGameEngine;
using NLog.Targets;

namespace Game.Skills
{
    public class Skill
    {
        private SkillData _data;
        private ISkillHandler _handler;


        public bool InUse => _handler != null && _handler.InUse;
        public SkillData Data => _data;

        public Skill(Component component, int skillID)
        {
            _data = SkillsDataStore.GetData(skillID);
            _handler = SkillsHandlerStore.CreateHandler(_data.skillType);
            component.InjectDependenciesIntoObject(_handler);
            _handler?.Init(component, _data);
        }

        public bool PreProcessSkill(GameObject target)
        {
           return _handler?.PreProcessSkill(target) ?? false;
        }
        public void PostProcessSkill() 
        {
            _handler?.PostProcessSkill();
        }
    }
}
