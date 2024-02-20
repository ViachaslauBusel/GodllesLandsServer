﻿using DataFileProtocol.Skills;
using Game.Animation;
using Game.Physics.Transform;
using Game.Skills.Commands;
using Game.Systems.Stats;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NLog.Targets;
using Protocol.Data.Replicated.Animation;
using Protocol.Data.Replicated.Transform;
using Protocol.Data.Stats;
using System.Numerics;

namespace Game.Skills.Handler
{
    public class MelleSkillHandler : ISkillHandler
    {
        private TransformComponent _transform;
        private AnimatorComponent _animator;
        private StatsComponent _stats;
        private MelleSkillData _data;
        private GameObject _target;

        public bool InUse => _target != null;

        public void Init(Component component, SkillData data)
        {
            _transform = component.GetComponent<TransformComponent>();
            _stats = component.GetComponent<StatsComponent>();
            _animator = component.GetComponent<AnimatorComponent>();
            _data = (MelleSkillData)data;
            if(_data == null)
            {
                Debug.Log.Error("MelleSkillHandler", "Data is not MelleSkillData");
            }
        }

        public bool PreProcessSkill(GameObject target)
        {
            if(InUse)
            {
                //Already in use
                return false;
            }
            if (target == null)
            {
                //TODO : Send error message to the client
                return false;
            }

            target.ReadData(out TransformData targetTransform);
            float distance = Vector3.Distance(_transform.Position, targetTransform.Position);

            if (distance > _data.range)
            {
                //TODO : Send error message to the client
                return false;
            }

            int stamina = _stats.GetStat(StatCode.Stamina);

            if (stamina < _data.staminaCost)
            {
                //TODO : Send error message to the client
                return false;
            }

            stamina -= _data.staminaCost;
            _stats.SetStat(StatCode.Stamina, stamina);

            _animator.Play(AnimationID.AttackType_1, AnimationLayer.TimeAnimation, (int)(_data.applyingTime * 1_000));

            _target = target;
            return true;
           
        }

        public void PostProcessSkill()
        {
            if(_target == null)
            {
                //Something gone wrong
                return;
            }

            _target.ReadData(out TransformData targetTransform);
            float distance = Vector3.Distance(_transform.Position, targetTransform.Position);

            if (distance > _data.range)
            {
                //TODO : Send error message to the client
                _target = null;
                return;
            }

            DamageCommand damageCommand = new DamageCommand();
            damageCommand.Attacker = _stats.GameObject;
            damageCommand.PAttack = 77 * _stats.GetStat(StatCode.MaxPAttack) + _data.damage;
            _target.SendCommand(damageCommand);

            _target = null;
        }
    }
}