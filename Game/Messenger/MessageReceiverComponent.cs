using Game.NetworkTransmission;
using NetworkGameEngine;
using NetworkGameEngine.Units.Characters;
using Protocol;
using Protocol.MSG.Game.Messenger;
using RUCP;

namespace Game.Messenger
{
    internal class MessageReceiverComponent : Component
    {
        private NetworkTransmissionComponent _networkTransmission;
        private MessageBroadcastComponent _messageBroadcast;

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_MESSAGE_CS, MessengerSendMessage);
        }

        private void MessengerSendMessage(Packet packet)
        {
            packet.Read(out MSG_MESSAGE_CS msg);

            _messageBroadcast.SendMessage(msg.Layer, msg.Message);
        }
    }
}
