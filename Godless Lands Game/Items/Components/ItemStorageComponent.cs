using Game.Items.Queries;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using Zenject;

namespace Game.Items.Components
{
    public class ItemStorageComponent : ItemStorageDbSyncComponent
    {
        private ItemUniqueIdGenerator _uniqueIdGenerator;
        private Dictionary<long, Item> _items;
        private CharacterInfoHolder _characterInfoHolder;

        protected override IEnumerable<Item> Items => _items.Values;
        public override bool HasDataToSave => _items.Values.Any(i => i.IsDataSyncWithDbPending) || _queries.Count > 0;

        public ItemStorageComponent()
        {
            _items = new Dictionary<long, Item>();
        }

        [Inject]
        private void InjectServices(ItemUniqueIdGenerator uniqueIdGenerator)
        {
            _uniqueIdGenerator = uniqueIdGenerator;
        }

        public override void Init()
        {
            base.Init();
            _characterInfoHolder = GetComponent<CharacterInfoHolder>();
        }

        protected override void LoadItem(Item item)
        {
            _items.Add(item.UniqueID, item);
        }

        internal long AddItem(Item item)
        {
            if (item.UniqueID == 0)
            {
                item.SetUniqueID(_uniqueIdGenerator.GetNextUniqueId());
                Debug.Log.Debug($"[{_characterInfoHolder.CharacterName}] Generating unique id for item:{item.Data.ID}:{item.UniqueID}");
            }

            if (_items.ContainsKey(item.UniqueID))
            {
                //Debug.Log.Fatal($"Cannot add item:{item.UniqueID} because it is already in storage");
                return item.UniqueID;
            }

            Debug.Log.Debug($"[{_characterInfoHolder.CharacterName}] Adding item:{item.UniqueID}");
            item.SetOwner(_characterInfoHolder.CharacterID);

            _items.Add(item.UniqueID, item);
            return item.UniqueID;
        }

        internal Item GetItem(long uid)
        {
           return _items.TryGetValue(uid, out Item item) ? item : null;
        }

        internal Job DestroyItem(Item item)
        {
           return DestroyItem(item?.UniqueID ?? 0);
        }

        internal async Job DestroyItem(long itemUID)
        {
            if (itemUID == 0)
            {
                Debug.Log.Warn($"Cannot destroy item:{itemUID} because it has no unique id");
                return;
            }

            Debug.Log.Debug($"[{_characterInfoHolder.CharacterName}] Destroying item:{itemUID}");
            var query = new DestroyItemQuery(_characterInfoHolder.CharacterID, itemUID);
            AddQuery(query);
            _items.Remove(itemUID);

            await new WaitUntil(() => query.IsDone);
        }

        internal bool ContainsItem(long uid) => _items.ContainsKey(uid);
    }
}
