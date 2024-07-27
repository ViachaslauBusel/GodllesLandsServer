using Game.GameObjectFactory;
using Game.Resources;
using Game.Tools;
using NetworkGameEngine;
using Protocol.Data.Resources;

namespace Game.Units.Resources
{
    internal static class ResourcesDataRegistry
    {
        private static Dictionary<int, ResourceInfo> _resourceData = new Dictionary<int, ResourceInfo>();

        public static void LoadData()
        {
            List<ResourceInfo> miningStones = JsonReader.Read<List<ResourceInfo>>(Path.Combine(ResourceFile.Folder, ResourceFile.MiningStones));

            if (miningStones == null)
            {
                Console.WriteLine("No mining stones found");
                return;
            }

            foreach (ResourceInfo miningStone in miningStones)
            {
                _resourceData.Add(miningStone.ID, miningStone);
            }
        }

        public static ResourceInfo GetResourceData(int id)
        {
            if (_resourceData.ContainsKey(id))
            {
                return _resourceData[id];
            }
            return null;
        }
    }
}
