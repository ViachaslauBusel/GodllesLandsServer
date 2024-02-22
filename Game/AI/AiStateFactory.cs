using Game.AI.States;
using Game.AI.StateTransitionConditions;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.AI
{
    public class AiStateFactory
    {
        private AiControllerComponent _aiController;

        public AiStateFactory(AiControllerComponent controllerComponent)
        {
            _aiController = controllerComponent;
        }

        public BaseState CreateState(AiState state, int AiType)
        {
            switch (state)
            {
                case AiState.Idle when AiType == 1:
                    return Inject(new IdleState(_aiController))
                        .AttachCondition(Inject(new ToAttackStateCondition(_aiController)))
                        .AttachCondition(Inject(new ToDeathStateCondition(_aiController)));
                case AiState.Patrol when AiType == 1:
                    return Inject(new PatrolState(_aiController))
                         .AttachCondition(Inject(new ToAttackStateCondition(_aiController)))
                         .AttachCondition(Inject(new ToDeathStateCondition(_aiController)));
                case AiState.Attacking when AiType == 1:
                    return Inject(new AttackState(_aiController))
                         .AttachCondition(Inject(new ToDeathStateCondition(_aiController)));
                case AiState.Chase when AiType == 1:
                    return Inject(new ChaseState(_aiController))
                        .AttachCondition(Inject(new ToDeathStateCondition(_aiController)));
                case AiState.Death when AiType == 1:
                    return Inject(new DeathState(_aiController));
                default:
                    Debug.Log.Error($"Can't create AI state");
                    return null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T Inject<T>(T instance)
        {
            return _aiController.InjectDependenciesIntoObject(instance);        }
    }
}
