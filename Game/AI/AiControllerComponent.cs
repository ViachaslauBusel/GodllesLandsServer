using Game.AI.States;
using Game.AI.StateTransitionConditions;
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
        private AiStateFactory _stateFactory;

        public AiControllerComponent()
        {
            _stateFactory = new AiStateFactory(this);
        }

        public override void Start()
        {
            _states.Add(AiState.Idle, _stateFactory.CreateState(AiState.Idle, 1));
            _states.Add(AiState.Patrol, _stateFactory.CreateState(AiState.Patrol, 1));
            _states.Add(AiState.Attack, _stateFactory.CreateState(AiState.Attacking, 1));
            _states.Add(AiState.Chase, _stateFactory.CreateState(AiState.Chase, 1));
            SetState(AiState.Idle);
        }

        private void SetState(AiState state)
        {
            if(_states.ContainsKey(state) == false)
            {
                Debug.Log.Error("State not found: " + state);
                return;
            }

            _currentState?.Deactivate();
            _currentState = _states[state];
            _currentState.Activate();
        }

        public override void Update()
        {
            if(_currentState.IsStateChangeRequired(out AiState newState))
            {
                SetState(newState);
            }
        }
    }
}
