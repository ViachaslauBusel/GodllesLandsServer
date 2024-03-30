using Godless_Lands_Game.ObjectInteraction;
using NetworkGameEngine;

namespace Game.Drop
{
    public class DropInteractionComponent : InteractiveObjectInteractionControllerComponent
    {

        protected override bool CanStartInteraction(IPlayerNetworkProfile playerProfile)
        {
            return true;
        }
    }
}
