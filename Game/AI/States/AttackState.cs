using Game.Physics.Transform;
using Game.Skills;
using Game.Systems.Stats;
using Game.Systems.Target;
using Protocol.Data.Replicated.Transform;
using System.Diagnostics;
using System.Numerics;

namespace Game.AI.States
{
    internal class AttackState : BaseState
    {
        private TargetManagerComponent _targetManager;
        private TransformComponent _transform;
        private SkillUsageComponent _skillUsage;

        public AttackState(AiControllerComponent component) : base(component)
        {
            _targetManager = component.GetComponent<TargetManagerComponent>();
            _transform = component.GetComponent<TransformComponent>();
            _skillUsage = component.GetComponent<SkillUsageComponent>();
        }

        public override bool Update(out AiState newState)
        {
            newState = AiState.Idle;
            if(_skillUsage.SkillInUse != null && _skillUsage.SkillInUse.InUse)
            {
                return false;
            }

            if(_targetManager.Target == null || _targetManager.Target.IsDestroyed)
            {
                _targetManager.SetTarget(null);
                newState = AiState.Idle;
                return true;
            }

            _targetManager.Target.ReadData(out HealtData healt);
            if(healt.HP == 0)
            {
                _targetManager.SetTarget(null);
                newState = AiState.Idle;
                return true;
            }

            _targetManager.Target.ReadData(out TransformData targetTransform);
            float distance = Vector3.Distance(_transform.Position, targetTransform.Position);
            if(distance > 3.0f)
            {
                newState = AiState.Chase;
                return true;
            }

            _skillUsage.UseSkill(1);
            return false;
        }
    }
}