using Game.GridMap;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using RUCP;
using Zenject;

namespace Game.NetworkTransmission
{
    internal class PacketDistributorComponent : Component
    {
        private IGridMapService m_gridMapService;

        [Inject]
        public void Construct(IGridMapService mapService)
        {
            m_gridMapService = mapService;
        }

        public void SendAraound(Packet packet, int range)
        {
            var players = m_gridMapService.GetPlayersAround(GameObject.ID, range);
            foreach (var player in players)
            {
                Debug.Log.Debug("Send packet to " + player.Socket.ID);
                player.Socket.Send(packet);
            }
        }
    }
}
