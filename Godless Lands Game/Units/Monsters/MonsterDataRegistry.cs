using Game.GameObjectFactory;
using Game.Resources;
using Game.Tools;
using NetworkGameEngine;
using Protocol.Data.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Units.Monsters
{
    public static class MonsterDataRegistry
    {
        private static Dictionary<int, MonsterInfo> _monsterData = new Dictionary<int, MonsterInfo>();
        public static void LoadData()
        {
            List<MonsterInfo> monsters = JsonReader.Read<List<MonsterInfo>>(Path.Combine(ResourceFile.Folder, ResourceFile.MonstersData));

            if (monsters == null)
            {
                Console.WriteLine("No monsters found");
                return;
            }

            foreach (MonsterInfo monster in monsters)
            {
                _monsterData.Add(monster.ID, monster);
            }
        }

        public static MonsterInfo GetMonsterData(int id)
        {
            if (_monsterData.ContainsKey(id))
            {
                return _monsterData[id];
            }
            return null;
        }
    }
}
