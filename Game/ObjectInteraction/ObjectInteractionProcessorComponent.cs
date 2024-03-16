using Game.NetworkTransmission;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.Data.Replicated.ObjectInteraction;
using Protocol.MSG.Game.ObjectInteraction;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction
{
    public class ObjectInteractionProcessorComponent : Component, IReactCommand<EndObjectInteractionCommand>
    {
        private NetworkTransmissionComponent _networkTransmission;
        private int _interactObjectId = -1;
        private bool _inProgress = false;

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
        }

        public override void Start()
        {
            _networkTransmission.RegisterHandler(Opcode.MSG_OBJECT_INTERACTION_REQUEST, OnObjectInteractionRequest);
        }
        public void ReactCommand(ref EndObjectInteractionCommand command)
        {
            if (IsInteractionInProgress() || CanProcessCommand(command.ObjectId) == false)
            {
                return;
            }

            EndInteraction();
        }

        private async void OnObjectInteractionRequest(Packet packet)
        {
            if (IsInteractionInProgress() || _interactObjectId != -1)
            {
                LogError("Object interaction request already in progress");
                return;
            }

            StartInteraction();

            packet.Read(out MSG_OBJECT_INTERACTION_REQUEST_CS request);

            if (!TryGetGameObject(request.ObjectId, out GameObject gameObject))
            {
                EndInteraction();
                return;
            }

            var cmd = new RequestObjectInteractionCommand { PlayerProfile = _networkTransmission.PlayerProfile };
            var job = gameObject.SendCommandAndReturnResult<RequestObjectInteractionCommand, bool>(cmd, 100);

            await job;

            EndInteraction();

            if (job.IsFaulted)
            {
                LogError("Error while processing object interaction request");
                return;
            }

            _interactObjectId = request.ObjectId;

            Debug.Log.Info($"[OBJECT_INTERACTION_REQUEST]: GameObject with id {request.ObjectId} found");
        }

        private bool CanProcessCommand(int objectId)
        {
            if (_interactObjectId != objectId)
            {
                LogError("Object id does not match the current interaction object id");
                return false;
            }

            return true;
        }

        private bool IsInteractionInProgress()
        {
            return _inProgress;
        }

        private void StartInteraction()
        {
            _inProgress = true;
        }

        private bool TryGetGameObject(int objectId, out GameObject gameObject)
        {
            if (GameObject.World.TryGetGameObject(objectId, out gameObject) == false)
            {
                LogError($"GameObject with id {objectId} not found");
                _interactObjectId = -1;
                return false;
            }

            return true;
        }

        private void EndInteraction()
        {
            _inProgress = false;
            _interactObjectId = -1;
        }

        private void LogError(string message)
        {
            Debug.Log.Error($"[OBJECT_INTERACTION_REQUEST]: {message}");
        }

    }
}
