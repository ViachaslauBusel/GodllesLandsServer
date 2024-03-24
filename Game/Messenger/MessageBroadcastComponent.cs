using Game.NetworkTransmission;
using NetworkGameEngine;
using NetworkGameEngine.Units.Characters;
using Protocol;
using Protocol.MSG.Game.Messenger;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Messenger
{
    public class MessageBroadcastComponent : Component
    {
        private PacketDistributorComponent _packetDistributor;
        private CharacterInfoHolder _characterInfoHolder;
        private NetworkTransmissionComponent _networkTransmission;

        public override void Init()
        {
            _packetDistributor = GetComponent<PacketDistributorComponent>();
            _characterInfoHolder = GetComponent<CharacterInfoHolder>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
        }

        public void SendMessage(MsgLayer layer, string message)
        {
            MSG_MESSAGE_SC response = new MSG_MESSAGE_SC
            {
                SenderName = _characterInfoHolder.CharacterName,
                Layer = layer,
                Message = message,
            };
            Packet packetResponse = Packet.Create(Channel.Reliable);
            packetResponse.OpCode = Opcode.MSG_MESSAGE_SC;
            packetResponse.Write(response);

            switch (layer)
            {
                case MsgLayer.AllMsg:
                    break;
                case MsgLayer.AroundMsg:
                    _packetDistributor.SendAraound(packetResponse, 1);
                    break;
                case MsgLayer.System:
                    _networkTransmission.Socket.Send(packetResponse);
                    break;
            }
        }
    }
}
