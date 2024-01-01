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
        private Client m_socket;
        private HandlersStorage<Action<Profile, Packet>> m_handlersStorage;
        private Dictionary<short, Action<Packet>> m_handlers = new Dictionary<short, Action<Packet>>();
        private ConcurrentQueue<Packet> m_packets = new ConcurrentQueue<Packet>();

        public Client Socket => m_socket;

        public NetworkTransmissionComponent(Client client, HandlersStorage<Action<Profile, Packet>> handlersStorage)
        {
            m_socket = client;
            m_handlersStorage = handlersStorage;
        }

        public override void Update()
        {
            while (m_packets.TryDequeue(out var packet))
            {
                if (m_handlers.TryGetValue(packet.OpCode, out var handler))
                {
                    handler(packet);
                }
                else
                {
                    Debug.Log.Fatal($"handler for opcode {packet.OpCode} not found");
                }
            }
        }

        private void HandlePacket(Profile profile, Packet packet)
        {
           m_packets.Enqueue(packet);
        }

        internal void RegisterHandler(short opcode, Action<Packet> handler)
        {
            if (m_handlers.ContainsKey(opcode))
            {
                Debug.Log.Fatal($"handler for opcode {opcode} already registered");
                return;
            }

            m_handlers.Add(opcode, handler);
            m_handlersStorage.RegisterHandler(opcode, HandlePacket);
        }

        public void UnregisterHandler(short opcode)
        {
            m_handlersStorage.UnregisterHandler(opcode);
        }
    }
}
