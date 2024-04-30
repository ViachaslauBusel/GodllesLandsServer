using Game.Physics;
using Game.Physics.Transform;
using Game.Tools;
using NetworkGameEngine;
using System.Numerics;
using Zenject;

namespace Game.AI.States
{
    internal class IdleState : BaseState
    {
        private RaycastingService _raycastingService;
        private TransformComponent _transformComponent;
        private GameObject gameObject;
        private long _endAwaitTime;

        public IdleState(AiControllerComponent component) : base(component)
        {
            _transformComponent = component.GetComponent<TransformComponent>();
        }

        [Inject]
        private void InjectServices(RaycastingService raycastingService)
        {
            _raycastingService = raycastingService;
        }

        public override void OnActive()
        {
           _endAwaitTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + RandomHelper.Range(3_000, 10_000);
        }

        public override bool Update(out AiState newState)
        {
            newState = AiState.Patrol;

            return _endAwaitTime < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}