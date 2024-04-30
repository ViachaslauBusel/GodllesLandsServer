using Game.Systems.Stats.Components;
using Godless_Lands_Game.ObjectInteraction;
using NetworkGameEngine;

namespace Game.ObjectInteraction.MiningStone
{
    public class MiningStoneInteractionComponent : InteractiveObjectInteractionControllerComponent
    {
        private BodyComponent _bodyComponent;

        public override void Init()
        {
            _bodyComponent = GetComponent<BodyComponent>();
            OnClientConnected += OnClientConnectedHandler;
        }

        private void OnClientConnectedHandler(GameObject playerCharacterObj, IPlayerNetworkProfile profile, List<Component> components)
        {
            components.Add(new CharacterMiningController(GameObject));
        }
        
        protected override bool CanStartInteraction(IPlayerNetworkProfile playerProfile)
        {
            if (_bodyComponent.IsAlive == false)
            {
                return false;
            }

            return true;
        }
    }
}
