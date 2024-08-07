﻿using Game.GridMap.Scripts;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Systems.Target.Commands;
using Game.Tools;
using Game.Units.Monsters.Components;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AI.States
{
    public class DeathState : BaseState
    {
        private TargetManagerComponent _targetManager;
        private BodyComponent _bodyComponent;
        private SpawnComponent _spawnComponent;
        private bool _needDoResurrect;
        private long _needDoResurrectTime;

        public DeathState(AiControllerComponent component) : base(component)
        {
            _targetManager = component.GetComponent<TargetManagerComponent>();
            _bodyComponent = component.GetComponent<BodyComponent>();
            _spawnComponent = component.GetComponent<SpawnComponent>();
        }

        public override void OnActive()
        {
           // Debug.Log.Debug($"Entity {_owner.GameObject.ID} is dead");
            _targetManager.SetTarget(null);


            _needDoResurrect = true;
            _needDoResurrectTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + _spawnComponent.MinSpawnTime + RandomHelper.Range(0, _spawnComponent.MaxSpawnTime);
        }

        public override void OnDeactive()
        {
            Debug.Log.Debug($"Entity {_owner.GameObject.ID} is alive");

            if (_owner.GetComponent<EntityTagComponent>() == null)
                _owner.GameObject.AddComponent(new EntityTagComponent());
        }

        public override bool Update(out AiState newState)
        {
            newState = AiState.Idle;

            if(_needDoResurrect && _needDoResurrectTime < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                Debug.Log.Debug($"Entity {_owner.GameObject.ID} is revive");
                _spawnComponent.Respawn();
                _needDoResurrect = false;
            }

            return _bodyComponent.IsAlive;
        }
    }
}
