using Game.ObjectInteraction;
using Game.Physics;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Tools;
using Game.Units.Monsters.Components;
using NetworkGameEngine;
using NetworkGameEngine.JobsSystem;
using Protocol.Data.SpawnData;
using System.Numerics;
using Zenject;

namespace Game.Units.Resources
{
    internal class RespawnComponent : SpawnComponent
    {
        private BodyComponent _bodyComponent;
        private bool _inProgress = false;


        public RespawnComponent(SpawnUnitPointData spawnPoint) : base(spawnPoint)
        {
        }

        public override void Init()
        {
            _bodyComponent = GetComponent<BodyComponent>();

            _bodyComponent.OnDeath += OnDeath;

            UpdatePosition();
        }

        private async void OnDeath()
        {
            if (_inProgress) return;
            _inProgress = true;

            int delay = MinSpawnTime + RandomHelper.Range(0, MaxSpawnTime);
            await new MillisDelayJob(delay);

            Respawn();
            _inProgress = false;
        }
    }
}
