using DataFileProtocol.Skills;
using Game.Animation;
using Game.Messenger;
using Game.Physics.Transform;
using Game.Skills.Commands;
using Game.Systems.Stats.Components;
using Game.Tools;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NLog.Targets;
using Protocol.Data.Replicated.Animation;
using Protocol.Data.Replicated.Transform;
using Protocol.Data.Stats;
using Protocol.MSG.Game.Messenger;
using System.Numerics;

namespace Game.Skills.Handler
{
    public class MelleSkillHandler : ISkillHandler
    {
        private MessageBroadcastComponent _messageBroadcast;
        private BodyComponent _body;
        private TransformComponent _transform;
        private AnimatorComponent _animator;
        private StatsComponent _stats;
        private MelleSkillData _data;
        private GameObject _target;

        public bool InUse => _target != null;

        public void Init(Component component, SkillData data)
        {
            _messageBroadcast = component.GetComponent<MessageBroadcastComponent>();
            _body = component.GetComponent<BodyComponent>();
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
                _messageBroadcast?.SendMessage(MsgLayer.System, "Skill is already in use");
                return false;
            }

            if (_body.IsAlive == false)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "You are dead");
                return false;
            }

            if (target == null)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "Target is null");
                return false;
            }

            target.ReadData(out TransformData targetTransform);
            Vector3 direction = targetTransform.Position - _transform.Position;
            float distance = direction.Length();
            direction = direction.Normalize(distance);

            if (distance > _data.range)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, $"Target is too far. {distance.ToString(".00")} > {_data.range.ToString(".00")}");
                return false;
            }

            int stamina = _stats.GetStat(StatCode.Stamina);

            if (stamina < _data.staminaCost)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "Not enough stamina");
                return false;
            }

            stamina -= _data.staminaCost;
            _stats?.SetStat(StatCode.Stamina, stamina);

            _animator.Play(AnimationID.AttackType_1, AnimationLayer.TimeAnimation, (int)(_data.applyingTime * 1_000), direction);

            _target = target;
            return true;
           
        }

        public async void PostProcessSkill()
        {
            if(_target == null)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "Target is null");
                return;
            }

            if(_body.IsAlive == false)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "You are dead");
                return;
            }

            _target.ReadData(out TransformData targetTransform);
            float distance = Vector3.Distance(_transform.Position, targetTransform.Position);

            if (distance > _data.range + 1f)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, $"[2] Target is too far. {distance.ToString(".00")} > {_data.range.ToString(".00")}");
                _target = null;
                return;
            }

            DamageCommand damageCommand = new DamageCommand();
            damageCommand.Attacker = _stats.GameObject;
            damageCommand.PAttack = 77 * RandomHelper.Range(_stats.GetStat(StatCode.MinPattack), _stats.GetStat(StatCode.MaxPAttack)) + _data.damage;

            GameObject target = _target;
            _target = null;

            var result = await target.SendCommandAndReturnResult<DamageCommand, DamageResponse>(damageCommand);

            _messageBroadcast?.SendMessage(MsgLayer.System, $"You hit the target for {result.Damage} damage");
        }
    }
}
