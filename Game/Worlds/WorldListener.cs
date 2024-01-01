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
using NetworkGameEngine.Debugger;
using Game.Replication.Scripts;
using Protocol.MSG.Game;
using Game.GridMap.Scripts;
using Protocol.Data.Replicated.Transform;
using Game.GameObjectFactory;

namespace NetworkGameEngine.Worlds
{
    static class WorldListener
    {
        [Handler(Opcode.MSG_WORLD_ENTRANCE)]
        public static async void Entrance(Profile profile, Packet packet)
        {
            //if (!GameLoop.MainWorld.TryGetGameObject(profile.CharacterObjectID, out GameObject character))
            //{
            //    Debug.Log.Fatal($"unable to find character with specified id");
            //    return;
            //}
            //while (!character.IsInitialized) { await Task.Delay(1); }


            GameObject character = CharacterFactory.Create(profile.SelectedChacterID, profile.Owner, profile.handlersStorage);

            profile.CharacterObjectID = await GameLoop.MainWorld.AddGameObject(character);


          
        }
    }
}
