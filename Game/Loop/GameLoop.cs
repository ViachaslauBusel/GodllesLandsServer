using Game.Replication;
using NetworkGameEngine;
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
        public static IReplicationService ReplicationService { get; } = new ReplicationService();
        public static void Start()
        {

            MainWorld.Init(8);
            Thread thread = new Thread(Loop);
            thread.Start();
        }

        private static void Loop()
        {
            while (true)
            {
                MainWorld.Update();

                Thread.Sleep(100);
            }
        }
    }
}
