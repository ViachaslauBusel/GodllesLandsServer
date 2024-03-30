using Game.NetworkTransmission;
using Godless_Lands_Game.UnitVisualization;
using NetworkGameEngine;
using NetworkGameEngine.JobsSystem;
using Protocol;
using Protocol.MSG.Game.Workbench;
using RUCP;

namespace Godless_Lands_Game.ObjectInteraction.Workbench
{
    public class WorkbenchInteractionControllerComponent : Component
    {
        private WorkbenchInteractionComponent _workbenchInteraction;
        private WorkbenchViewComponent _workbenchView;
        private PlayersNetworkTransmissionComponent _playersNetworkTransmission;

        public override void Init()
        {
            _workbenchInteraction = GetComponent<WorkbenchInteractionComponent>();
            _workbenchView = GetComponent<WorkbenchViewComponent>();
            _playersNetworkTransmission = GetComponent<PlayersNetworkTransmissionComponent>();

            _workbenchInteraction.OnClientConnected += OnClientConnected;
            _workbenchInteraction.OnClientDisconnected += OnClientDisconnected;
        }

        private async void OnClientConnected(GameObject characterObj, IPlayerNetworkProfile profile, List<Component> components)
        {
            _playersNetworkTransmission.RegisterHandler(profile, Opcode.MSG_WORKBENCH_CLOSE_WINDOW, OnCloseWindow);

            ToggleWorkbenchWindow(profile, true, false);

            var controller = new CharacterSmelterControllerComponent(_workbenchView.WorkbenchType);
            components.Add(controller);
            await new WaitUntil(() => controller.IsReadyForWork);

            if (_workbenchInteraction.IsClientConnected(profile))
            { ToggleWorkbenchWindow(profile, true, true); }
        }

        /// <summary>
        /// Request from the client to close the workbench window
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="packet"></param>
        private void OnCloseWindow(IPlayerNetworkProfile profile, Packet packet)
        {
            _playersNetworkTransmission.UnregisterHandler(profile, Opcode.MSG_WORKBENCH_CLOSE_WINDOW);
            _workbenchInteraction.DisconnectClient(profile.CharacterObjectID);
        }

        private void OnClientDisconnected(GameObject @object, IPlayerNetworkProfile profile)
        {
             ToggleWorkbenchWindow(profile, false, false);
        }

        private void ToggleWorkbenchWindow(IPlayerNetworkProfile profile, bool isOpen, bool isReadyForWok)
        {
            MSG_WORKBENCH_TOGGLE_WINDOW_SC msg = new MSG_WORKBENCH_TOGGLE_WINDOW_SC();
            msg.IsOpen = isOpen;
            msg.IsReadyForWork = isReadyForWok;
            msg.WorkbenchType = _workbenchView.WorkbenchType;
            profile.Client.Send(msg);
        }
    }
}
