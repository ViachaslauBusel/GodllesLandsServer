using Game.AI.States;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AI
{
    public class AiControllerComponent : Component
    {
        private Dictionary<AiState, BaseState> _states = new Dictionary<AiState, BaseState>();
        private BaseState _currentState;

        public override void Start()
        {
            _states.Add(AiState.Idle, InjectDependenciesIntoObject(new IdleState(this)));
            _states.Add(AiState.Patrol, InjectDependenciesIntoObject(new PatrolState(this)));
            _states.Add(AiState.Attack, InjectDependenciesIntoObject(new AttackState(this)));
            SetState(AiState.Idle);
        }

        private void SetState(AiState state)
        {
            if(_states.ContainsKey(state) == false)
            {
                Debug.Log.Error("State not found: " + state);
                return;
            }

            _currentState?.Deactive();
            _currentState = _states[state];
            _currentState.Active();
        }

        public override void Update()
        {
            if(_currentState.Update(out AiState newState))
            {
                SetState(newState);
            }
        }
    }
}
