using Game.GridMap.Scripts;
using Game.Systems.Stats;
using Game.Systems.Target;
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
        private bool _needDoRemoveFromMap;
        private long _removeFromMapTime;
        private bool _needDoResurrect;
        private long _needDoResurrectTime;

        public DeathState(AiControllerComponent component) : base(component)
        {
            _targetManager = component.GetComponent<TargetManagerComponent>();
            _bodyComponent = component.GetComponent<BodyComponent>();
        }

        public override void OnActive()
        {
            Debug.Log.Debug($"Entity {_owner.GameObject.ID} is dead");
            _targetManager.SetTarget(null);


            // Нужно сделать воскрешение через 10 секунд
            _needDoResurrect = true;
            _needDoResurrectTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 10_000;
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
                _bodyComponent.Revive();
                _needDoResurrect = false;
            }

            return _bodyComponent.IsAlive;
        }
    }
}
