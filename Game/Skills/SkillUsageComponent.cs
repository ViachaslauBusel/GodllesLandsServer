﻿using Game.NetworkTransmission;
using Game.Systems.Target;
using Game.Systems.TargetSystem;
using NetworkGameEngine;
using NetworkGameEngine.ContinuationTaskExecution;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsManagment;
using Protocol.MSG.Game.Skills;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Skills
{
    public class SkillUsageComponent : Component
    {
        protected SkillsStoreComponent _skillsStore;
        protected TargetManagerComponent _targetManager;
        private Skill _skilInUse, _nextSkill;

        public Skill SkillInUse => _skilInUse;

        public override void Start()
        {
            _targetManager = GetComponent<TargetManagerComponent>();
            _skillsStore = GetComponent<SkillsStoreComponent>();
        }
        
        public void UseSkill(int skillId)
        {
            Skill skill = _skillsStore.GetSkill(skillId);

            if(skill == null) 
            {
                Debug.Log.Error($"[{GameObject.Name}:{GameObject.ID}]Skill:{skillId} not found");
                return;
            }

            UseSkill(skill);
        }

        private async void UseSkill(Skill skill)
        {
            if (skill == null) return;

            if (_skilInUse != null && _skilInUse.InUse)
            {
                _nextSkill = skill;
                return;
            }

            if (skill.PreProcessSkill(_targetManager.Target))
            {
                _skilInUse = skill;
                await new SecondsDelayJob(_skilInUse.Data.applyingTime);
                _skilInUse.PostProcessSkill();
            }

            Skill nextSkill = _nextSkill;
            _skilInUse = null;
            _nextSkill = null;
            UseSkill(nextSkill);
        }
    }
}