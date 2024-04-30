using DataFileProtocol.Skills;
using Game.NetworkTransmission;
using Game.Skills.Data;
using Game.Skills.Handler;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.MSG.Game.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Skills
{
    public class SkillsStoreComponent : Component
    {
        protected Dictionary<int, Skill> _skills = new Dictionary<int, Skill>();
       

        public override void Init()
        {
            _skills.Add(1, new Skill(this, 1));
            _skills.Add(2, new Skill(this, 2));
            _skills.Add(5, new Skill(this, 5));
        }

        internal Skill GetSkill(int skillID)
        {
            if (_skills.ContainsKey(skillID) == false)
            {
                Debug.Log.Warn("SkillsStoreComponent", "Skill with id {0} not found", skillID);
                return null;
            }

            return _skills[skillID];
        }
    }
}
