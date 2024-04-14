using DataFileProtocol.Skills;
using Game.Animation;
using Game.CombatModeControl.Components;
using Game.Messenger;
using Game.NetworkTransmission;
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
using Protocol.MSG.Game.CombatMode;
using Protocol.MSG.Game.Messenger;
using Protocol.MSG.Game.Test;
using System.Numerics;

namespace Game.Skills.Handler
{
    public class MelleSkillHandler : ISkillHandler
    {
        private NetworkTransmissionComponent _networkTransmission;
        private MessageBroadcastComponent _messageBroadcast;
        private BodyComponent _body;
        private TransformComponent _transform;
        private AnimatorComponent _animator;
        private StatsComponent _stats;
        private CombatModeComponent _combatMode;
        private MelleSkillData _data;
        private GameObject _target;

        public bool InUse => _target != null;

        public void Init(Component component, SkillData data)
        {
            _networkTransmission = component.GetComponent<NetworkTransmissionComponent>();
            _messageBroadcast = component.GetComponent<MessageBroadcastComponent>();
            _body = component.GetComponent<BodyComponent>();
            _transform = component.GetComponent<TransformComponent>();
            _stats = component.GetComponent<StatsComponent>();
            _animator = component.GetComponent<AnimatorComponent>();
            _combatMode = component.GetComponent<CombatModeComponent>(); 

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

            if (_combatMode != null && _combatMode.CombatMode == false)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "You are not in combat mode");
                return false;
            }

            if (target == null)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "Target is null");
                return false;
            }

            // You can't attack yourself
            if (target == _body.GameObject)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "Invalid target");
                return false;
            }

            target.ReadData(out TransformData targetTransform);
            Vector3 direction = targetTransform.Position.ClearY() - _transform.Position.ClearY();
            float distance = direction.Length();
            direction = direction.Normalize(distance);

            //if(_networkTransmission != null)//Test
            //{
            //    MSG_DRAW_POINTS msg = new MSG_DRAW_POINTS();
            //    msg.Points = new List<Point>();
            //    msg.Points.Add(new Point() { Position = _transform.Position, Color = PointColor.Green });
            //    msg.Points.Add(new Point() { Position = targetTransform.Position, Color = PointColor.Red });
            //    _networkTransmission.Socket.Send(msg);
            //}

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

            _animator.Play((AnimationID)_data.animationId, AnimationLayer.TimeAnimation, _data.usingTime, direction);

            _target = target;
            return true;
           
        }

        public async void ApplySkill()
        {
            if (_target == null)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "Target is null");
                return;
            }

            if (_body.IsAlive == false)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, "You are dead");
                return;
            }

            // Считывание местоположения обьекта по которому будет произведен удар из потоко безопасного буфера
            _target.ReadData(out TransformData targetTransform);
            float distance = Vector3.Distance(_transform.Position.ClearY(), targetTransform.Position.ClearY());

            if (distance > _data.range + 1f)
            {
                _messageBroadcast?.SendMessage(MsgLayer.System, $"[2] Target is too far. {distance.ToString(".00")} > {_data.range.ToString(".00")}");
                return;
            }

            // Создание команды, которая будет наносить урон
            DamageCommand damageCommand = new DamageCommand();
            damageCommand.Attacker = _stats.GameObject;
            damageCommand.PAttack = 77 * RandomHelper.Range(_stats.GetStat(StatCode.MinPattack), _stats.GetStat(StatCode.MaxPattack)) + _data.damage;

            // Отправка команды на стадии Update() игровому объекту, по которому будет произведен удар, и получение результата с нанесенным уроном
            var result = await _target.SendCommandAndReturnResult<DamageCommand, DamageResponse>(damageCommand, 500);

            // Этот код будет выполнен в этом же тике на стадии Job
            _messageBroadcast?.SendMessage(MsgLayer.System, $"You hit the target for {result.Damage} damage");
        }

        public void PostProcessSkill()
        {
            GameObject target = _target;
            _target = null;
        }
    }
}
