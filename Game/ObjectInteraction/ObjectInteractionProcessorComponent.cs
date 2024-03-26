using Game.Messenger;
using Game.NetworkTransmission;
using Game.ObjectInteraction.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.MSG.Game.Messenger;
using Protocol.MSG.Game.ObjectInteraction;
using RUCP;

namespace Game.ObjectInteraction
{
    public class ObjectInteractionProcessorComponent : Component, IReactCommand<InteractionEndNotificationCommand>
    {
        private MessageBroadcastComponent _messageBroadcast;
        private NetworkTransmissionComponent _networkTransmission;
        private GameObject _interactObject = null;
        private bool _inProgress = false;

        public event Action<int> OnInteractionStarted;
        public event Action<int> OnInteractionEnded;

        public int InteractObjectId => _interactObject != null ? _interactObject.ID : -1;

        public override void Init()
        {
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
        }

        public override void Start()
        {
            _networkTransmission.RegisterHandler(Opcode.MSG_OBJECT_INTERACTION_REQUEST, HandleObjectInteractionRequest);
        }

        private void HandleObjectInteractionRequest(Packet packet)
        {
            packet.Read(out MSG_OBJECT_INTERACTION_REQUEST_CS request);
            InitiateInteraction(request.ObjectId);
        }

        /// <summary>
        /// Cancel object interaction
        /// This command is sent by the object when the interaction has been completed or canceled
        /// </summary>
        /// <param name="command"></param>
        public void ReactCommand(ref InteractionEndNotificationCommand command)
        {
            // if has interaction in progress or object id does not match the current interaction object id
            if (IsHasActiveProcces() || CanProcessCommandFrom(command.ObjectId) == false)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Object interaction has been completed. Notify");
                return;
            }

            _messageBroadcast.SendMessage(MsgLayer.System, "Object interaction has been completed");
            EndInteractionWithObject();
        }

        // Send request to the object for interaction
        public async void InitiateInteraction(int objectId)
        {
            if (IsHasActiveProcces() || _interactObject != null)
            {
               // LogError("Object interaction request already in progress");
                _messageBroadcast.SendMessage(MsgLayer.System, "You are already interacting with another object");
                return;
            }

            BeginInteractionProcess();
        

            if (GameObject.World.TryGetGameObject(objectId, out GameObject gameObject) == false)
            {
                EndInteractionProccess();
                return;
            }

            var cmd = new RequestObjectInteractionCommand
            {
                PlayerCharacterObject = GameObject,
                PlayerProfile = _networkTransmission.PlayerProfile,
                State = InteractionState.StartInteraction
            };
            long startTime = Time.Milliseconds;
            var interactionJob = gameObject.SendCommandAndReturnResult<RequestObjectInteractionCommand, bool>(cmd, 100);

            await interactionJob;

            _messageBroadcast.SendMessage(MsgLayer.System, $"Object interaction request has been completed in {Time.Milliseconds - startTime} ms");

            EndInteractionProccess();

            if (interactionJob.IsFaulted || interactionJob.GetResult() == false)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Failed to interact with the object");
                return;
            }

            _interactObject = gameObject;

            _messageBroadcast.SendMessage(MsgLayer.System, "Object interaction has been started");
            OnInteractionStarted?.Invoke(objectId);
        }

        
        public async void StopInteraction(int objectId)
        {
            if (IsHasActiveProcces() || CanProcessCommandFrom(objectId) == false)
            {
                LogError("Object interaction request already in progress");
                return;
            }

            BeginInteractionProcess();

            var cmd = new RequestObjectInteractionCommand
            {
                PlayerCharacterObject = GameObject,
                PlayerProfile = _networkTransmission.PlayerProfile,
                State = InteractionState.EndInteraction
            };

            long startTime = Time.Milliseconds;
            bool result = await _interactObject.SendCommandAndReturnResult<RequestObjectInteractionCommand, bool>(cmd, 200);

            if(result == false) LogError("Failed to stop interaction with the object");

            EndInteractionProccess();
            EndInteractionWithObject();
            _messageBroadcast.SendMessage(MsgLayer.System, $"Object interaction has been stopped in {Time.Milliseconds - startTime} ms");
            OnInteractionEnded?.Invoke(objectId);
        }

        private bool CanProcessCommandFrom(int objectId)
        {
            if (InteractObjectId != objectId)
            {
                LogError("Object id does not match the current interaction object id");
                return false;
            }

            return true;
        }

        private bool IsHasActiveProcces()
        {
            return _inProgress;
        }

        private void BeginInteractionProcess()
        {
            _inProgress = true;
        }

        private void EndInteractionProccess()
        {
            _inProgress = false;
        }

        private void EndInteractionWithObject()
        {
            _interactObject = null;
        }

        private void LogError(string message)
        {
            Debug.Log.Error($"[OBJECT_INTERACTION_REQUEST]: {message}");
        }
    }
}
