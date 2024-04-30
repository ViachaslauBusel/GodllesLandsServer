using Game.Messenger;
using Game.NetworkTransmission;
using Game.ObjectInteraction.Commands;
using Godless_Lands_Game.ObjectInteraction;
using Godless_Lands_Game.ObjectInteraction.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.MSG.Game.Messenger;
using Protocol.MSG.Game.ObjectInteraction;
using RUCP;

namespace Game.ObjectInteraction
{
    /// <summary>
    /// This component on the player character object is responsible for processing object interaction requests.
    /// </summary>
    public class ObjectInteractionProcessorComponent : Component, IReactCommand<InteractionEndNotificationCommand>
    {
        private MessageBroadcastComponent _messageBroadcast;
        private NetworkTransmissionComponent _networkTransmission;
        private InteractionObjectContext _interactionContext;
        private bool _inProgress = false;

        public event Action<int> OnInteractionStarted;
        public event Action<int> OnInteractionEnded;

        public int InteractObjectId => _interactionContext.InteractObjectId;
        public bool IsBusy => _inProgress || _interactionContext.HasInteraction;

        public override void Init()
        {
            _interactionContext = new InteractionObjectContext(GameObject);
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
            _interactionContext.EndInteractionWithObject();
        }

        // Send request to the object for interaction
        public async void InitiateInteraction(int objectId)
        {
            if (IsHasActiveProcces() || _interactionContext.HasInteraction)
            {
               // LogError("Object interaction request already in progress");
                _messageBroadcast.SendMessage(MsgLayer.System, "You are already interacting with another object");
                return;
            }

            Debug.Log.Info($"Initiate interaction with object {objectId}");
            BeginInteractionProcess();
        

            if (GameObject.World.TryGetGameObject(objectId, out GameObject gameObject) == false)
            {
                Debug.Log.Warn($"Object with id {objectId} not found");
                EndInteractionProccess();
                return;
            }

            var cmd = new InteractionStartRequestCommand
            {
                PlayerCharacterObject = GameObject,
                PlayerProfile = _networkTransmission.PlayerProfile,
            };
            //long startTime = Time.Milliseconds;
            var interactionJob = gameObject.SendCommandAndReturnResult<InteractionStartRequestCommand, InteractionStartConfirmation>(cmd, 100);

            await interactionJob;

            //_messageBroadcast.SendMessage(MsgLayer.System, $"Object interaction request has been completed in {Time.Milliseconds - startTime} ms");

            EndInteractionProccess();

            if (interactionJob.IsFaulted)
            {
                Debug.Log.Error("Failed to interact with the object");
                _messageBroadcast.SendMessage(MsgLayer.System, "Failed to interact with the object");
                return;
            }

            InteractionStartConfirmation confirmation = interactionJob.GetResult();

            if(confirmation.Result == false)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Failed to interact with the object");
                return;
            }

            _interactionContext.BeginInteractionWithObject(gameObject, confirmation.Components);

            Debug.Log.Info($"Object interaction with object {objectId} has been started");
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

            Debug.Log.Info($"Stop interaction with object {objectId}");
            BeginInteractionProcess();

            var cmd = new InteractionEndRequestCommand
            {
                PlayerCharacterObjectID = GameObject.ID,
            };

            //long startTime = Time.Milliseconds;
            bool result = await _interactionContext.Object.SendCommandAndReturnResult<InteractionEndRequestCommand, bool>(cmd, 200);

            if(result == false) LogError("Failed to stop interaction with the object");

            EndInteractionProccess();
            _interactionContext.EndInteractionWithObject();
            //Debug.Log.Info($"Object interaction with object {objectId} has been stopped");
            //_messageBroadcast.SendMessage(MsgLayer.System, $"Object interaction has been stopped in {Time.Milliseconds - startTime} ms");
            OnInteractionEnded?.Invoke(objectId);
        }

        private bool CanProcessCommandFrom(int objectId)
        {
            if (InteractObjectId != objectId)
            {
                LogError($"Object id:{objectId} does not match the current interaction object id:{InteractObjectId}");
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



        private void LogError(string message)
        {
            Debug.Log.Error($"[OBJECT_INTERACTION_REQUEST]: {message}");
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_OBJECT_INTERACTION_REQUEST);

            if (_interactionContext.HasInteraction)
            {
                StopInteraction(_interactionContext.InteractObjectId);
            }
        }
    }
}
