using Game.GridMap;
using Game.Monsters;
using Game.Pathfinding;
using Game.Physics;
using Game.Replication;
using NetworkGameEngine;
using RUCP;

namespace Game.Loop
{
    public static class GameLoop
    {
        public static World MainWorld { get; } = new World();
        public static PhysicWorld PhysicWorld { get; } = new PhysicWorld();

        private static Server m_server;
        private static IGridMapService m_gridService;
        private static IReplicationService m_replicationService;
        private static RaycastingService m_raycastingService;

        public static void Start(Server server)
        {
            PhysicWorld.Load();

            m_server = server;
            m_gridService = new GridMapService(3000, 100);
            m_replicationService = new ReplicationService(m_gridService);
            m_raycastingService = new RaycastingService(PhysicWorld);
            

            MainWorld.RegisterService<IGridMapService>(m_gridService);
            MainWorld.RegisterService<IReplicationService>(m_replicationService);
            MainWorld.RegisterService<RaycastingService>(m_raycastingService);
            MainWorld.RegisterService<Pathfinder>(new Pathfinder());
           
            MainWorld.Init(8);
         
            Thread thread = new Thread(Loop);
            thread.Start();

            MonsterSpawner.SpawnMonster(MainWorld);
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
