using Game.ObjectInteraction;
using Game.Physics;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Tools;
using NetworkGameEngine;
using NetworkGameEngine.JobsSystem;
using System.Numerics;
using Zenject;

namespace Game.Units.MiningStones
{
    internal class RespawnComponent : Component
    {
        private RaycastingService _raycastingService;
        private TransformComponent _transformComponent;
        private BodyComponent _bodyComponent;
        private Vector3 _spawnPoint;
        private float _spawnRadius;
        private int _minSpawnTime;
        private int _maxSpawnTime;
        private bool _inProgress = false;

        [Inject]
        public void InjectServices(RaycastingService raycastingService)
        {
            _raycastingService = raycastingService;
        }

        public RespawnComponent(Vector3 spawnPoint, float spawnRadius, int startSpawnTime, int timeSpawn)
        {
            _spawnPoint = spawnPoint;
            _spawnRadius = spawnRadius;
            _minSpawnTime = startSpawnTime;
            _maxSpawnTime = startSpawnTime + timeSpawn;
        }

        public override void Init()
        {
            _transformComponent = GetComponent<TransformComponent>();
            _bodyComponent = GetComponent<BodyComponent>();

            _bodyComponent.OnDeath += OnDeath;

            SetPosition();
        }

        private async void OnDeath()
        {
            if (_inProgress) return;
            _inProgress = true;

            int delay = RandomHelper.Range(_minSpawnTime, _maxSpawnTime);
            await new MillisDelayJob(delay);

            Respawn();
            _inProgress = false;
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
