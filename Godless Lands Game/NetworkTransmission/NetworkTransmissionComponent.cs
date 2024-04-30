using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using RUCP;
using RUCP.Handler;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.NetworkTransmission
{
    public class NetworkTransmissionComponent : Component
    {
        private IPlayerNetworkProfile _playerProfile;
        private PacketProcessing<Action<Packet>> _packetProcessing;

        public IPlayerNetworkProfile PlayerProfile => _playerProfile;
        public Client Socket => _playerProfile.Client;

        public NetworkTransmissionComponent(IPlayerNetworkProfile playerProfile)
        {
            _playerProfile = playerProfile;
            _packetProcessing = new PacketProcessing<Action<Packet>>(playerProfile);
        }

        public override void Update()
        {
           _packetProcessing.ProcessPackets();
        }

        public void RegisterHandler(short opcode, Action<Packet> handler) => _packetProcessing.RegisterHandler(opcode, handler);

        public void UnregisterHandler(short opcode) => _packetProcessing.UnregisterHandler(opcode);
    }
}
