using ModestTree.Util;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using RUCP;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Game.NetworkTransmission
{
    public class PacketProcessing<T>
    {
        private Dictionary<short, T> _handlers = new Dictionary<short, T>();
        private ConcurrentQueue<Packet> _packets = new ConcurrentQueue<Packet>();
        private IPlayerNetworkProfile _playerProfile;
        private Action<Packet, T> _localHandler;

        public int HandlerCount => _handlers.Count;

        public PacketProcessing(IPlayerNetworkProfile playerProfile)
        {
            Debug.Log.Info($"Create new PacketProcessing for {playerProfile.CharacterObjectID}");
            _playerProfile = playerProfile;
            if (typeof(T) == typeof(Action<Packet>))
            {
                _localHandler = (p, h) => ((Action<Packet>)(object)h)(p);
            }
            else if (typeof(T) == typeof(Action<IPlayerNetworkProfile, Packet>))
            {
                _localHandler = (p, h) => ((Action<IPlayerNetworkProfile, Packet>)(object)h)(_playerProfile, p);
            }
            else
            {
                Debug.Log.Fatal($"handler type {typeof(T)} not supported");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ProcessPackets()
        {
            while (_packets.TryDequeue(out var packet))
            {
                if (_handlers.TryGetValue(packet.OpCode, out var handler))
                {
                    _localHandler(packet, handler);
                }
                else
                {
                    Debug.Log.Fatal($"handler for opcode {packet.OpCode} not found");
                }
            }
        }

        /// <summary>
        /// Here the RUCP thread is working, which processes the packets
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="packet"></param>
        private void HandlePacket(PlayerProfile profile, Packet packet)
        {
            _packets.Enqueue(packet);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RegisterHandler(short opcode, T handler)
        {
            if (_handlers.ContainsKey(opcode))
            {
                Debug.Log.Fatal($"handler for opcode {opcode} already registered");
                return;
            }

            _handlers.Add(opcode, handler);
            _playerProfile.HandlersStorage.RegisterHandler(opcode, HandlePacket);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnregisterHandler(short opcode)
        {
            if (_handlers.Remove(opcode) == false)
            {
                Debug.Log.Fatal($"handler for opcode {opcode} not found");
                return;
            }
            _handlers.Remove(opcode);
            _playerProfile.HandlersStorage.UnregisterHandler(opcode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnregisterAllHandlers()
        {
            foreach (var handler in _handlers)
            {
                _playerProfile.HandlersStorage.UnregisterHandler(handler.Key);
            }
            _handlers.Clear();
        }
    }
}
