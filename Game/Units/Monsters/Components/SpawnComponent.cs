using Game.Physics;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Tools;
using NetworkGameEngine;
using System.Numerics;
using Zenject;

namespace Game.Units.Monsters.Components
{
    public class SpawnComponent : Component
    {
        private RaycastingService _raycastingService;
        private TransformComponent _transformComponent;
        private BodyComponent _bodyComponent;
        private Vector3 _spawnPoint;
        private float _spawnRadius;

        [Inject]
        public void InjectServices(RaycastingService raycastingService)
        {
            _raycastingService = raycastingService;
        }

        public SpawnComponent(Vector3 spawnPoint, float spawnRadius)
        {
            _spawnPoint = spawnPoint;
            _spawnRadius = spawnRadius;
        }

        public override void Init()
        {
            _transformComponent = GetComponent<TransformComponent>();
            _bodyComponent = GetComponent<BodyComponent>();
            SetPosition();
        }

        private void SetPosition()
        {
            _raycastingService.GetTerrainPoint(GetRandomPosition(), out Vector3 position);
            _transformComponent.UpdatePosition(position);
        }

        internal void Respawn()
        {
            _bodyComponent.Revive();
            SetPosition();
        }

        private Vector3 GetRandomPosition()
        {
            return _spawnPoint + new Vector3(RandomHelper.Range(-_spawnRadius, _spawnRadius), 0, RandomHelper.Range(-_spawnRadius, _spawnRadius));
        }
    }
}
