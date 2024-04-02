using Game.Physics.Transform;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Units.NPCs.Components
{
    public class NpcSpawnComponent : Component
    {
        private Vector3 _spawnPoint;
        private float _spawnRotation;
        private TransformComponent _transform;

        public NpcSpawnComponent(Vector3 spawnPoint, float spawnRotation)
        {
            _spawnPoint = spawnPoint;
            _spawnRotation = spawnRotation;
        }

        public override void Init()
        {
            _transform = GetComponent<TransformComponent>();
            _transform.UpdatePosition(_spawnPoint);
            _transform.UpdateRotation(_spawnRotation);
        }
    }
}
