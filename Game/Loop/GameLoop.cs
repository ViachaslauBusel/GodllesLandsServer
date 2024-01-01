using Game.GridMap;
using Game.Replication;
using Game.Systems.TargetSystem;
using NetworkGameEngine;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Loop
{
    public static class GameLoop
    {
        public static World MainWorld { get; } = new World();

        private static Server m_server;
        private static IGridMapService m_gridService;
        private static IReplicationService m_replicationService;

        public static void Start(Server server)
        {
            m_server = server;
            m_gridService = new GridMapService(3000, 100);
            m_replicationService = new ReplicationService(m_gridService);

            MainWorld.RegisterService<IGridMapService>(m_gridService);
            MainWorld.RegisterService<IReplicationService>(m_replicationService);
           
            MainWorld.Init(8);
         
            Thread thread = new Thread(Loop);
            thread.Start();
        }

        private static void Loop()
        {
            while (true)
            {
                //m_server.ProcessPacket();
                MainWorld.Update();

                m_gridService.Update();
                m_replicationService.Update();

                //m_server.DistributePackets();

                Thread.Sleep(100);
            }
        }
    }
}
