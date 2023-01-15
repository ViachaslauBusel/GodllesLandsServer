using RUCP.Handler;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using Game.Commands;
using Game.Loop;
using Game.Replication;

namespace NetworkGameEngine.Worlds
{
    static class WorldListener
    {
        [Handler(Opcode.MSG_WORLD_ENTRANCE)]
        public static async void Entrance(Profile profile, Packet packet)
        {
            if (!GameLoop.MainWorld.TryGetGameObject(profile.CharacterObjectID, out GameObject character))
            {
                return;
            }
            while (!character.isInitialized) { await Task.Delay(1); }

            character.AddComponent(new ReplicationTagComponent());
        }
    }
}
