using Game.Physics;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Tools;
using NetworkGameEngine;
using Protocol.Data.SpawnData;
using System.Numerics;
using Zenject;

namespace Game.Units.Monsters.Components
{
    public class SpawnComponent : Component
    {
        private RaycastingService _raycastingService;
        private TransformComponent _transformComponent;
        private BodyComponent _bodyComponent;
        private SpawnPointType _spawnType;
        private Vector3 _spawnPoint;
        private float _spawnRadius;
        private int _minSpawnTime;
        private int _maxSpawnTime;

        public int MinSpawnTime => _minSpawnTime;
        public int MaxSpawnTime => _maxSpawnTime;

        [Inject]
        public void InjectServices(RaycastingService raycastingService)
        {
            _raycastingService = raycastingService;
        }

        public SpawnComponent(SpawnUnitPointData pointData)
        {
            _spawnType = pointData.SpawnType;
            _spawnPoint = pointData.Position;
            _spawnRadius = pointData.SpawnRadius;
            _minSpawnTime = pointData.MinSpawnTime;
            _maxSpawnTime = pointData.MaxSpawnTime;
        }

        public override void Init()
        {
            _transformComponent = GetComponent<TransformComponent>();
            _bodyComponent = GetComponent<BodyComponent>();
            UpdatePosition();
        }

        protected void UpdatePosition()
        {
            if(_spawnType == SpawnPointType.Point)
            {
                _transformComponent.UpdatePosition(_spawnPoint);
                _transformComponent.UpdateRotation(_spawnRadius);
            }
            else
            {
                _raycastingService.GetTerrainPoint(GetRandomPosition(), out Vector3 position);
                _transformComponent.UpdatePosition(position);
            }
          
        }

        internal void Respawn()
        {
            _bodyComponent.Revive();
            UpdatePosition();
        }

        private Vector3 GetRandomPosition()
        {
            switch (_spawnType)
            {
                case SpawnPointType.Circle:
                    return GetRandomCirclePosition();
                    case SpawnPointType.Square:
                    return GetRandomSquarePosition();
                default: return _spawnPoint;
            }
        }

        private Vector3 GetRandomSquarePosition()
        {
            return _spawnPoint + new Vector3(RandomHelper.Range(-_spawnRadius, _spawnRadius), 0, RandomHelper.Range(-_spawnRadius, _spawnRadius));
        }

        private Vector3 GetRandomCirclePosition()
        {
            Vector3 randomDirection = new Vector3(RandomHelper.Range(-1, 1), 0, RandomHelper.Range(-1, 1));
            randomDirection = Vector3.Normalize(randomDirection);
            return _spawnPoint + randomDirection * RandomHelper.Range(0, _spawnRadius);
        }
    }
}
