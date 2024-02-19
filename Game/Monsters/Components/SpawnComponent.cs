using Game.Physics.Transform;
using Game.Tools;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Monsters.Components
{
    public class SpawnComponent : Component
    {
        private TransformComponent _transformComponent;
        private Vector3 _spawnPoint;
        private float _spawnRadius;

        public SpawnComponent(Vector3 spawnPoint, float spawnRadius)
        {
            _spawnPoint = spawnPoint;
            _spawnRadius = spawnRadius;
        }

        public override void Init()
        {
            _transformComponent = GetComponent<TransformComponent>();
            Vector3 position = GetRandomPosition();
            _transformComponent.UpdatePosition(position);
            Debug.Log.Debug($"SpawnComponent Start {position}");
        }

        public override void Start()
        {
           
        }

        private Vector3 GetRandomPosition()
        {
            return _spawnPoint + new Vector3(RandomHelper.Range(-_spawnRadius, _spawnRadius), 0, RandomHelper.Range(-_spawnRadius, _spawnRadius));
        }
    }
}
