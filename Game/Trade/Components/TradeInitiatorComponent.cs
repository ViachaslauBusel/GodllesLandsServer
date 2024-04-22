using Game;
using Game.Messenger;
using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Game.Physics.Transform;
using Godless_Lands_Game.GameObjectFactory;
using Godless_Lands_Game.Trade.Commands;
using NetworkGameEngine;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using Protocol;
using Protocol.Data.Replicated.Transform;
using Protocol.MSG.Game.Messenger;
using Protocol.MSG.Game.Trade;
using RUCP;
using System.Numerics;

namespace Godless_Lands_Game.Trade.Components
{
    public class TradeInitiatorComponent : Component, IReactCommandWithResult<TradeResponseCommand, GameObject>
    {
        private TransformComponent _transform;
        private NetworkTransmissionComponent _networkTransmission;
        private MessageBroadcastComponent _messageBroadcast;
        private CharacterInfoHolder _characterInfoHolder;
        private ObjectInteractionProcessorComponent _interactionProcessor;
        private bool _inProgress = false;

        public override void Init()
        {
            _transform = GetComponent<TransformComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();
            _characterInfoHolder = GetComponent<CharacterInfoHolder>();
            _interactionProcessor = GetComponent<ObjectInteractionProcessorComponent>();


            _networkTransmission.RegisterHandler(Opcode.MSG_TRADE_REQUEST, HandleTradeRequest);
        }

        // Receive request from the client to start the trade
        private async void HandleTradeRequest(Packet packet)
        {
            if(_interactionProcessor.IsBusy)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "You are busy");
                return;
            }
            if (_inProgress)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Trade in progress");
                return;
            }

            packet.Read(out MSG_TRADE_REQUEST_CS msg);

            if (msg.TargetID == GameObject.ID || msg.TargetID == 0)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Wrong target");
                return;
            }

            GameObject.World.TryGetGameObject(msg.TargetID, out GameObject target);

            if (target == null)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Target not found");
                return;
            }

            //Check distance
            target.ReadData(out TransformData targetTransform);
            if(Vector3.Distance(_transform.Position, targetTransform.Position) > 10f)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Target is too far");
                return;
            }

            _inProgress = true;

            var tradeRequestCommand = new TradeRequestCommand(GameObject, _characterInfoHolder.CharacterName);
            bool result = await target.SendCommandAndReturnResult<TradeRequestCommand, bool>(tradeRequestCommand, 3_000);
        
            if(result == false)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Trade request failed");
                _inProgress = false;
                return;
            }
            
            StartWaitingProcess();
        }

        private async void StartWaitingProcess()
        {
            long endTime = Time.Milliseconds + 20_000;
            await new WaitUntil(() => endTime < Time.Milliseconds || _inProgress == false);
            _inProgress = false;
        }

        public GameObject ReactCommand(ref TradeResponseCommand command)
        {
           _inProgress = false;

            if(command.Result == false)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Trade was rejected");
                return null;
            }

            _messageBroadcast.SendMessage(MsgLayer.System, "Trade was accepted");
            GameObject tradeObject = TradeObjectFactory.CreateTradeObject(GameObject, command.TradeAcceptor);
            GameObject.World.AddGameObject(tradeObject);

            StartInteraction(tradeObject);

            return tradeObject;
        }

        private async void StartInteraction(GameObject tradeObject)
        {
            await new WaitWhile(() => tradeObject.ID == 0);
            _interactionProcessor.InitiateInteraction(tradeObject.ID);
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_TRADE_REQUEST);
        }
    }
}
