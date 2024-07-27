using Game.GameObjectFactory;
using Game.Resources;
using Game.Tools;
using Game.Units.Resources;
using Game.Units.Monsters;
using Godless_Lands_Game.GameObjectFactory;
using Godless_Lands_Game.Units.NPCs;
using Protocol.Data.Resources;
using Protocol.Data.Monsters;
using Protocol.Data.NPCs;
using Protocol.Data.SpawnData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Spawner
{
    public static class UnitSpawnController
    {
        public static void SpawnAllUnits(NetworkGameEngine.World mainWorld)
        {
            List<SpawnUnitPointData> spawnPoints = JsonReader.Read<List<SpawnUnitPointData>>(Path.Combine(ResourceFile.Folder, ResourceFile.UnitsSpawnPoint));

            if (spawnPoints == null)
            {
                Console.WriteLine("No spawn points found");
                return;
            }

            foreach(var point in spawnPoints)
            {
                if (point.UnitType == UnitType.Monster)
                {
                    MonsterInfo monsterData = MonsterDataRegistry.GetMonsterData(point.UnitID);
                    if (monsterData != null)
                    {
                        mainWorld.AddGameObject(MonsterFactory.CreateMonster(monsterData, point));
                    }
                }
                else if(point.UnitType == UnitType.NPC)
                {
                    NpcInfo npcData = NPCsDataRegistry.GetNpcData(point.UnitID);
                    if (npcData != null)
                    {
                        mainWorld.AddGameObject(NPCsFactory.CreateNPC(npcData, point));
                    }
                }
                else if(point.UnitType == UnitType.Resource)
                {
                    ResourceInfo resourceData = ResourcesDataRegistry.GetResourceData(point.UnitID);
                    if (resourceData != null)
                    {
                        mainWorld.AddGameObject(ResourceStoneFactory.Create(resourceData, point));
                    }
                }
            }
        }
    }
}
