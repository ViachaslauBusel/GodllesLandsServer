using Game.GridMap;
using Game.Items;
using Game.Pathfinding;
using Game.Physics;
using Game.Replication;
using Game.RespawnPoints;
using Game.Units.MiningStones;
using Game.Units.Monsters;
using Godless_Lands_Game.Recipes;
using Godless_Lands_Game.Units.NPCs;
using Godless_Lands_Game.Units.Workbenches;
using Godless_Lands_Game.WorldEntry;
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
            MainWorld.OnLog += (message) => Debug.Log.Error(message);
            PhysicWorld.Load();
            Debug.Log.Info("Physic world loaded");

            m_server = server;
            m_gridService = new GridMapService(3000, 100);
            m_replicationService = new ReplicationService(m_gridService);
            m_raycastingService = new RaycastingService(PhysicWorld);
            var itemUniqueIdGenerator = new ItemUniqueIdGenerator();
            var playerWorldEntryController = new PlayerWorldEntryController();
            

            MainWorld.RegisterService<IGridMapService>(m_gridService);
            MainWorld.RegisterService<IReplicationService>(m_replicationService);
            MainWorld.RegisterService<RaycastingService>(m_raycastingService);
            MainWorld.RegisterService<Pathfinder>(new Pathfinder());
            MainWorld.RegisterService(itemUniqueIdGenerator);
            MainWorld.RegisterService(new ItemsFactory(itemUniqueIdGenerator));
            MainWorld.RegisterService(new RespawnPointsService());
            MainWorld.RegisterService(new RecipesDataStorageService());
            MainWorld.RegisterService(playerWorldEntryController);
           
           
            MainWorld.Init(8);
         
            Thread thread = new Thread(Loop);
            thread.Start();

            MonsterSpawner.SpawnMonster(MainWorld);
            MiningStoneSpawner.SpawnStones(MainWorld);
            WorkbenchSpawner.SpawnWorkbenches(MainWorld);
            NPCsSpawner.SpawnNPC(MainWorld);

            playerWorldEntryController.ActivateEntarnce();
        }

        private static void Loop()
        {
            Debug.Log.Info("Game loop started");

            while (true)
            {
                long startTickTime = Time.Milliseconds;

                UpdateGame();

                int sleepTime = CalculateSleepTime(startTickTime);
                if (sleepTime > 0)
                    Thread.Sleep(sleepTime);
            }
        }

        private static void UpdateGame()
        {
            Time.NextTick();
            MainWorld.Update();
            m_gridService.Update();
            m_replicationService.Update();
        }

        private static int CalculateSleepTime(long startTickTime)
        {
            return (int)(Time.fixedDeltaTimeMillis - (Time.Milliseconds - startTickTime));
        }
    }
}
