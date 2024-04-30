using Game.Pathfinding;
using Game.Physics.Transform;
using Game.Systems.Target;
using Protocol.Data.Replicated.Transform;
using System.Numerics;
using Zenject;

namespace Game.AI.States
{
    public class ChaseState : BaseState
    {
        private Pathfinder m_pathfinder;
        private TransformComponent m_transform;
        private UnitPathMoverComponent m_unitPathMover;
        private TargetManagerComponent m_targetManager;
        private Vector3[] m_path;

        public ChaseState(AiControllerComponent component) : base(component)
        {
            m_transform = component.GetComponent<TransformComponent>();
            m_unitPathMover = component.GetComponent<UnitPathMoverComponent>();
            m_targetManager = component.GetComponent<TargetManagerComponent>();
        }

        [Inject]
        private void InjectServices(Pathfinder pathfinder)
        {
            m_pathfinder = pathfinder;
        }

        public override void OnDeactive()
        {
            base.OnDeactive();
            m_path = null;
            m_unitPathMover.StopMove();
        }

        public override bool Update(out AiState newState)
        {
            if(m_targetManager.Target == null || m_targetManager.Target.IsDestroyed)
            {
                m_targetManager.SetTarget(null);
                newState = AiState.Idle;
                return true;
            }

            m_targetManager.Target.ReadData(out TransformData enemyTransform);
            float distance = Vector3.Distance(m_transform.Position, enemyTransform.Position);
            if(distance < 2.5f)
            {
                newState = AiState.Attack;
                return true;
            }

            if(m_unitPathMover.IsMove == false || m_path == null || Vector3.Distance(enemyTransform.Position, m_path[^1]) > 1.5f)
            {
                m_path = m_pathfinder.CalculatePath(m_transform.Position, enemyTransform.Position);
                if (m_path != null && m_path.Length > 1)
                {
                    m_unitPathMover.MoveAlongPath(m_path);
                }
            }

            newState = AiState.Idle;
            return false;
        }
    }
}
