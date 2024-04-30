using Game.Resources;
using NetworkGameEngine.Debugger;
using Newtonsoft.Json;
using Protocol.Data.Items;

namespace Game.Items
{
    public static class ItemsDataManager
    {
        private static Dictionary<int, ItemData> _items = new Dictionary<int, ItemData>();
        public static void Load()
        {
            string fullPath = Path.Combine(ResourceFile.Folder, ResourceFile.Items);
            if (File.Exists(fullPath))
            {
                string text = File.ReadAllText(fullPath);
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                List<ItemData> items = JsonConvert.DeserializeObject<List<ItemData>>(text, settings);
                for (int i = 0; i < items.Count; i++)
                {
                    ItemData item = items[i];
                    if (item == null)
                    {
                        Debug.Log.Error("ItemsDataManager", "Item with index {0} is null", i);
                        continue;
                    }
                    if (_items.ContainsKey(item.ID))
                    {
                        Debug.Log.Error("ItemsDataManager", "Item with id {0} already exists", item.ID);
                        continue;
                    }
                    _items.Add(item.ID, item);
                }
            }
            else Debug.Log.Fatal("ItemsDataManager", $"{fullPath} not found");
        }

        internal static ItemData GetData(int id)
        {
            if(_items.TryGetValue(id, out ItemData data))
            {
                return data;
            }
            else
            {
                Debug.Log.Error("ItemsDataManager", "Item with id {0} not found", id);
                return null;
            }
        }
    }
}
