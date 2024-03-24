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
    public static class MonsterSpawner
    {

        public static void SpawnMonster(World world)
        {
            List<MonsterData> monsters = JsonReader.Read<List<MonsterData>>(Path.Combine(ResourceFile.Folder, ResourceFile.Monsters));

            if (monsters == null)
            {
                Console.WriteLine("No monsters found");
                return;
            }

            foreach (MonsterData monster in monsters)
            {
                GameObject monsterObject = MonsterFactory.CreateMonster(monster);
                if (monsterObject != null)
                {
                    world.AddGameObject(monsterObject);
                }
            }
        }
    }
}
