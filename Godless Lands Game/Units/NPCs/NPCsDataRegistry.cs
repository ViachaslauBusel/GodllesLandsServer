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
    internal class NPCsDataRegistry
    {
        private static Dictionary<int, NpcInfo> _npcData = new Dictionary<int, NpcInfo>();
        public static void LoadData()
        {
            List<NpcInfo> npcs = JsonReader.Read<List<NpcInfo>>(Path.Combine(ResourceFile.Folder, ResourceFile.NPCsData));

            if (npcs == null)
            {
                Console.WriteLine("No npc found");
                return;
            }

            foreach (NpcInfo npc in npcs)
            {
                _npcData.Add(npc.ID, npc);
            }
        }

        public static NpcInfo GetNpcData(int id)
        {
            if (_npcData.ContainsKey(id))
            {
                return _npcData[id];
            }
            return null;
        }
    }
}
