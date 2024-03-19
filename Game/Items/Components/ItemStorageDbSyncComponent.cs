using Database;
using Game.DB;
using Game.Inventory.Components;
using Game.Items.Queries;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using Zenject;

namespace Game.Items.Components
{
    public struct ItemDbData
    {
        public long uid;
        public int id;
        public int count;
    }
    public abstract class ItemStorageDbSyncComponent : Component, IDatabaseReadable, IDatabaseWritable
    {
        private CharacterInfoHolder _characterInfoHolder;
        private ItemsFactory _itemsFactory;
        protected Queue<IQuery> _queries = new Queue<IQuery>();

        protected abstract IEnumerable<Item> Items { get; }

        public DatabaseSavePriority DatabaseSavePriority =>  DatabaseSavePriority.SuperHigh;
        public abstract bool HasDataToSave { get; }

        [Inject]
        private void InjectServices(ItemsFactory itemsFactory)
        {
            _itemsFactory = itemsFactory;
        }

        public override void Init()
        {
            _characterInfoHolder = GetComponent<CharacterInfoHolder>();
        }

        protected void AddQuery(IQuery query)
        {
            _queries.Enqueue(query);
        }

        public async Job ReadFromDatabaseAsync()
        {
            List<ItemDbData> items = await JobsManager.Execute(GameDatabaseProvider.Select<List<ItemDbData>>
             ($"SELECT load_items('{_characterInfoHolder.CharacterID}')"));

            if(items == null)
            {
                return;
            }

            foreach (var itemData in items)
            {
                Item item = _itemsFactory.CreateItem(itemData.id, itemData.uid, itemData.count);
                if (item == null)
                {
                    Debug.Log.Error($"Item:{itemData.uid}:{itemData.id} cannot be created");
                    continue;
                }

                LoadItem(item);
            }
        }

        protected abstract void LoadItem(Item item);

        public async Job<bool> WriteToDatabase()
        {
            List<Job<bool>> jobs = new List<Job<bool>>();
            foreach (var item in Items)
            {
                if (item.IsDataSyncWithDbPending)
                {
                    jobs.Add(
                        JobsManager.Execute(GameDatabaseProvider.SelectObject<bool>($"SELECT upsert_item('{_characterInfoHolder.CharacterID}', {item.UniqueID}, {item.Data.ID}, {item.Count})")));
                }
            }
            while (_queries.Count > 0)
            {
                jobs.Add(JobsManager.Execute(GameDatabaseProvider.SelectObject<bool>(_queries.Dequeue().Command)));
            }
            await Job.WhenAll(jobs);
            
            return jobs.All(j => j.GetResult());
        }
    }
}
