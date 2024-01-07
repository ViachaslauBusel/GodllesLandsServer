using DataFileProtocol.Skills;
using Game.Physics;
using Game.Skills.Commands;
using Game.Systems.Stats;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Replicated.Transform;
using Protocol.Data.Stats;
using System.Numerics;

namespace Game.Skills.Handler
{
    public class MelleSkillHandler : ISkillHandler
    {
        private TransformComponent _transform;
        private StatsComponent _stats;
        private MelleSkillData _data;

        public void Init(Component component, SkillData data)
        {
            _transform = component.GetComponent<TransformComponent>();
            _stats = component.GetComponent<StatsComponent>();
            _data = (MelleSkillData)data;
            if(_data == null)
            {
                Debug.Log.Error("MelleSkillHandler", "Data is not MelleSkillData");
            }
        }

        public void Use(GameObject target)
        {
            if (target == null)
            {
                //TODO : Send error message to the client
                return;
            }

            target.ReadData(out TransformData targetTransform);
            float distance = Vector3.Distance(_transform.Position, targetTransform.Position);

            if (distance > _data.range)
            {
                //TODO : Send error message to the client
                return;
            }

            int stamina = _stats.GetStat(StatCode.Stamina);

            if (stamina < _data.staminaCost)
            {
                //TODO : Send error message to the client
                return;
            }

            stamina -= _data.staminaCost;
            _stats.SetStat(StatCode.Stamina, stamina);

            DamageCommand damageCommand = new DamageCommand();
            damageCommand.PAttack = 77 * _stats.GetStat(StatCode.MaxPAttack) + _data.damage;
            target.SendCommand(damageCommand);
        }
    }
}
