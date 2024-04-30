using Game.GameObjectFactory;
using Game.Resources;
using Game.Tools;
using Godless_Lands_Game.GameObjectFactory;
using NetworkGameEngine;
using Protocol.Data.Monsters;
using Protocol.Data.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Units.NPCs
{
    internal class NPCsSpawner
    {
        public static void SpawnNPC(World world)
        {
            List<NpcData> npcs = JsonReader.Read<List<NpcData>>(Path.Combine(ResourceFile.Folder, ResourceFile.NPCs));

            if (npcs == null)
            {
                Console.WriteLine("No npc found");
                return;
            }

            foreach (NpcData npc in npcs)
            {
                GameObject npcObject = NPCsFactory.CreateNPC(npc);
                if (npcObject != null)
                {
                    world.AddGameObject(npcObject);
                }
            }
        }
    }
}
