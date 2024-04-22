using Game;
using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Godless_Lands_Game.Trade.Commands;
using NetworkGameEngine;
using NetworkGameEngine.JobsSystem;
using Protocol;
using Protocol.MSG.Game.Trade;
using RUCP;

namespace Godless_Lands_Game.Trade.Components
{
    public class TradeAcceptorComponent : Component, IReactCommandWithResult<TradeRequestCommand, bool>
    {
        private ObjectInteractionProcessorComponent _interactionProcessor;
        private NetworkTransmissionComponent _networkTransmission;
        private bool _inProgress = false;
        private GameObject _initiatorObject;

        public override void Init()
        {
            _interactionProcessor = GetComponent<ObjectInteractionProcessorComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_TRADE_REQUEST_RESPONSE, HandleTradeRequestResponse);
        }

        private async void HandleTradeRequestResponse(Packet packet)
        {
            _inProgress = false;
            packet.Read(out MSG_TRADE_REQUEST_RESPONSE_CS msg);

            GameObject tradeObject = await _initiatorObject.SendCommandAndReturnResult<TradeResponseCommand, GameObject>(new TradeResponseCommand(GameObject, msg.Result), 3_000);

            if(tradeObject != null)
            {
                await new WaitWhile(() => tradeObject.ID == 0);
                _interactionProcessor.InitiateInteraction(tradeObject.ID);
            }
            _initiatorObject = null;
        }

        public bool ReactCommand(ref TradeRequestCommand command)
        {
            if (_interactionProcessor.IsBusy || _inProgress) return false;

            _inProgress = true;
            _initiatorObject = command.InitiatorObject;

            // Send request to the client to start the trade
            MSG_TRADE_REQUEST_SC msg = new MSG_TRADE_REQUEST_SC();
            msg.RequesterName = GameObject.Name;
            msg.Time = 10_000;
            _networkTransmission.Socket.Send(msg);


            StartAwaitProcces();

            return true;
        }

        private async void StartAwaitProcces()
        {
            long endTime = Time.Milliseconds + 12_000;
            await new WaitUntil(() => endTime > Time.Milliseconds || _inProgress == false);

            _inProgress = false;
        }
    }
}
