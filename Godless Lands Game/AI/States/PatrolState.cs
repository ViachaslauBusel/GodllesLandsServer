using Game.Pathfinding;
using Game.Physics.Transform;
using Game.Tools;
using System.Numerics;
using Zenject;

namespace Game.AI.States
{
    internal class PatrolState : BaseState
    {
        private Pathfinder m_pathfinder;
        private TransformComponent m_transformComponent;
        private UnitPathMoverComponent m_unitPathMoverComponent;
        private Vector3 m_spawnPoint;

        public PatrolState(AiControllerComponent component) : base(component)
        {
            m_transformComponent = component.GetComponent<TransformComponent>();
            m_unitPathMoverComponent = component.GetComponent<UnitPathMoverComponent>();
            m_spawnPoint = m_transformComponent.Position;
        }

        [Inject]
        private void InjectServices(Pathfinder pathfinder)
        {
            m_pathfinder = pathfinder;
        }

        override public void OnActive()
        {
            Vector3 targetPoint = Vector3.Zero;
            do
            {
                targetPoint = m_spawnPoint + new Vector3(RandomHelper.Range(-10, 10), 0, RandomHelper.Range(-10, 10));
            }
            while (Vector3.Distance(m_transformComponent.Position, targetPoint) < 2);


            Vector3[] path = m_pathfinder.CalculatePath(m_transformComponent.Position, targetPoint);
            m_unitPathMoverComponent.MoveAlongPath(path);
        }

        public override bool Update(out AiState newState)
        {
            newState = AiState.Idle;

            return m_unitPathMoverComponent.IsMove == false;
        }

        public override void OnDeactive()
        {
            m_unitPathMoverComponent.StopMove();
        }
    }
}