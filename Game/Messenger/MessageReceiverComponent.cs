using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.Messenger;
using RUCP;

namespace Game.Messenger
{
    internal class MessageReceiverComponent : Component
    {
        private NetworkTransmissionComponent _networkTransmission;
        private PacketDistributorComponent m_packetDistributor;

        public override void Init()
        {
            m_packetDistributor = GetComponent<PacketDistributorComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _networkTransmission.RegisterHandler(Opcode.MSG_MESSAGE_CS, MessengerSendMessage);
        }

        private void MessengerSendMessage(Packet packet)
        {
            packet.Read(out MSG_MESSAGE_CS msg);

            MSG_MESSAGE_SC response = new MSG_MESSAGE_SC
            {
                SenderName = "test",
                Layer = msg.Layer,
                Message = msg.Message,
            };
            Packet packetResponse = Packet.Create(Channel.Reliable);
            packetResponse.OpCode = Opcode.MSG_MESSAGE_SC;
            packetResponse.Write(response);

            switch (msg.Layer)
            {
                case MsgLayer.AllMsg:
                    break;
                case MsgLayer.AroundMsg:
                    m_packetDistributor.SendAraound(packetResponse, 1);
                    break;
            }
        }
    }
}
