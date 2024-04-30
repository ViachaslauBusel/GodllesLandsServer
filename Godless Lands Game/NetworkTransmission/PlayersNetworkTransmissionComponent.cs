using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Game.NetworkTransmission
{
    public class PlayersNetworkTransmissionComponent : Component
    {
        private Dictionary<IPlayerNetworkProfile, PacketProcessing<Action<IPlayerNetworkProfile, Packet>>> _playersHandler = new ();

        public override void Update()
        {
            foreach (var playerHandler in _playersHandler)
            {
                playerHandler.Value.ProcessPackets();
            }
        }

        internal void RegisterHandler(IPlayerNetworkProfile playerProfile, short opcode, Action<IPlayerNetworkProfile, Packet> handler)
        {
            if (_playersHandler.ContainsKey(playerProfile) == false)
            {
               _playersHandler.Add(playerProfile, new (playerProfile));
            }

            _playersHandler[playerProfile].RegisterHandler(opcode, handler);
        }

        public void UnregisterHandler(IPlayerNetworkProfile playerProfile, short opcode)
        {
            if (_playersHandler.ContainsKey(playerProfile) == false)
            {
                Debug.Log.Error($"Player {playerProfile.CharacterObjectID} not found in PlayersNetworkTransmissionComponent");
                return;
            }

            _playersHandler[playerProfile].UnregisterHandler(opcode);

            if (_playersHandler[playerProfile].HandlerCount == 0)
            {
                Debug.Log.Info($"Player {playerProfile.CharacterObjectID} has no more handlers, removing from PlayersNetworkTransmissionComponent");
                _playersHandler.Remove(playerProfile);
            }
        }

        public void UnregisterAllHandlers(IPlayerNetworkProfile playerProfile)
        {
            if (_playersHandler.ContainsKey(playerProfile) == false) return;

            _playersHandler[playerProfile].UnregisterAllHandlers();
            _playersHandler.Remove(playerProfile);
        }
    }
}
