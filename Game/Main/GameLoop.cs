using Game.GridMap;
using Game.Items;
using Game.Pathfinding;
using Game.Physics;
using Game.Replication;
using Game.RespawnPoints;
using Game.Units.MiningStones;
using Game.Units.Monsters;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using RUCP;

namespace Game.Main
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
            Debug.Log.Info("Physic world loaded");

            m_server = server;
            m_gridService = new GridMapService(3000, 100);
            m_replicationService = new ReplicationService(m_gridService);
            m_raycastingService = new RaycastingService(PhysicWorld);
            var itemUniqueIdGenerator = new ItemUniqueIdGenerator();
            

            MainWorld.RegisterService<IGridMapService>(m_gridService);
            MainWorld.RegisterService<IReplicationService>(m_replicationService);
            MainWorld.RegisterService<RaycastingService>(m_raycastingService);
            MainWorld.RegisterService<Pathfinder>(new Pathfinder());
            MainWorld.RegisterService(itemUniqueIdGenerator);
            MainWorld.RegisterService(new ItemsFactory(itemUniqueIdGenerator));
            MainWorld.RegisterService(new RespawnPointsService());
           
           
            MainWorld.Init(8);
         
            Thread thread = new Thread(Loop);
            thread.Start();

            MonsterSpawner.SpawnMonster(MainWorld);
            MiningStoneSpawner.SpawnStones(MainWorld);
        }

        private static void Loop()
        {
            Debug.Log.Info("Game loop started");
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
