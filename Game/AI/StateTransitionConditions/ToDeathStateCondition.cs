using Game.Systems.Stats.Components;

namespace Game.AI.StateTransitionConditions
{
    public class ToDeathStateCondition : AIStateTransitionConditionBase
    {
        private BodyComponent _body;

        public ToDeathStateCondition(AiControllerComponent aiController)
        {
            _body = aiController.GetComponent<BodyComponent>();
        }

        public override bool CheckCondition(out AiState newState)
        {
            newState = AiState.Death;
            return _body.IsAlive == false;
        }
    }
}
