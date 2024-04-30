using NetworkGameEngine;

namespace Godless_Lands_Game.ObjectInteraction.Workbench
{
    public class WorkbenchInteractionComponent : InteractiveObjectInteractionControllerComponent
    {
        protected override bool CanStartInteraction(IPlayerNetworkProfile playerProfile)
        {
            return true;
        }
    }
}
