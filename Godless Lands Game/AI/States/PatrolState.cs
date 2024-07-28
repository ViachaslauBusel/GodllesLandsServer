using Game.Pathfinding;
using Game.Physics.Transform;
using Game.Tools;
using Game.Units.Monsters.Components;
using System.Numerics;
using Zenject;

namespace Game.AI.States
{
    internal class PatrolState : BaseState
    {
        private Pathfinder m_pathfinder;
        private TransformComponent m_transformComponent;
        private UnitPathMoverComponent m_unitPathMoverComponent;
        private SpawnComponent m_spawnComponent;

        public PatrolState(AiControllerComponent component) : base(component)
        {
            m_transformComponent = component.GetComponent<TransformComponent>();
            m_unitPathMoverComponent = component.GetComponent<UnitPathMoverComponent>();
            m_spawnComponent = component.GetComponent<SpawnComponent>();
        }

        [Inject]
        private void InjectServices(Pathfinder pathfinder)
        {
            m_pathfinder = pathfinder;
        }

        override public void OnActive()
        {
            Vector3 targetPoint = Vector3.Zero;
            bool isFind = false;
            for (int i = 0; i < 10; i++)
            {
                targetPoint = m_spawnComponent.GetRandomPosition();
                if(Vector3.Distance(m_transformComponent.Position, targetPoint) > 1)
                {
                    isFind = true;
                    break;
                }
            }

            if (isFind == false) return;

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