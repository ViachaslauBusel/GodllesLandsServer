using Game.GameObjectFactory;
using Game.Resources;
using Game.Tools;
using NetworkGameEngine;
using Protocol.Data.MiningStone;

namespace Game.Units.MiningStones
{
    internal static class MiningStoneSpawner
    {
        public static void SpawnStones(World world)
        {
            List<ResourceMiningStoneData> miningStones = JsonReader.Read<List<ResourceMiningStoneData>>(Path.Combine(ResourceFile.Folder, ResourceFile.MiningStones));

            if (miningStones == null)
            {
                Console.WriteLine("No mining stones found");
                return;
            }

            foreach (ResourceMiningStoneData miningStone in miningStones)
            {
                GameObject stoneObject = MiningStoneFactory.CreateStone(miningStone);
                if (stoneObject != null)
                {
                    world.AddGameObject(stoneObject);
                }
            }
        }
    }
}
