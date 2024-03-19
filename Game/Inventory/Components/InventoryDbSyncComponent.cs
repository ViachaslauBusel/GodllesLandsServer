using Database;
using Game.DB;
using Game.Items;
using Game.Items.Components;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using Protocol.MSG.Game.Inventory;
using Zenject;

namespace Game.Inventory.Components
{
    public struct CellDbData
    {
        public int bagIndex;
        public int slotIndex;
        public long uniqueId;
    }
    public class InventoryDbSyncComponent : Component, IDatabaseReadable, IDatabaseWritable
    {
        private CharacterInfoHolder _characterInfoHolder;
        private ItemStorageComponent _itemStorage;
        private InventoryComponent _inventory;
        private DBControlComponent _dbControl;
        private List<CellDbData> _items;

        public DatabaseSavePriority DatabaseSavePriority => DatabaseSavePriority.Hight;

        public bool HasDataToSave  => _inventory.PrimaryInventory.IsDataSyncWithDbPending || _inventory.SecondaryInventory.IsDataSyncWithDbPending;

        public override void Init()
        {
            _inventory = GetComponent<InventoryComponent>();
            _characterInfoHolder = GetComponent<CharacterInfoHolder>();
            _dbControl = GetComponent<DBControlComponent>();
            _itemStorage = GetComponent<ItemStorageComponent>();

            _dbControl.OnDatabaseLoadComplete += OnDatabaseLoadComplete;
        }

        private void OnDatabaseLoadComplete()
        {
            if(_items == null)
            {
                return;
            }
            foreach (var itemData in _items)
            {
                Item item = _itemStorage.GetItem(itemData.uniqueId);

                if(item == null)
                {
                    Debug.Log.Error($"Item:{itemData.uniqueId} cannot be found in the storage");
                    continue;
                }

                if (AddItemToInventory(item, itemData) == false)
                {
                    Debug.Log.Error($"Item:{itemData.uniqueId} cannot be added to the inventory");
                }
                _inventory.PrimaryInventory.MarkDataAsSyncedWithDb();
                _inventory.SecondaryInventory.MarkDataAsSyncedWithDb();
            }
            _items = null;
        }

        public async Job ReadFromDatabaseAsync()
        {
           _items = await JobsManager.Execute(GameDatabaseProvider.Select<List<CellDbData>>($"SELECT load_inventory('{_characterInfoHolder.CharacterID}')"));
        }

        private bool AddItemToInventory(Item item, CellDbData itemData)
        {
            if (itemData.bagIndex == (int)InventoryBagType.Primary)
            {
                return _inventory.PrimaryInventory.AddItem(item, itemData.slotIndex)
                    || _inventory.SecondaryInventory.AddItem(item);
            }
            else
            {
                return _inventory.SecondaryInventory.AddItem(item, itemData.slotIndex)
                    || _inventory.PrimaryInventory.AddItem(item);
            }
        }

        public async Job<bool> WriteToDatabase()
        {
            List<Job<bool>> jobs = new List<Job<bool>>();

            SyncBagData(jobs, _inventory.PrimaryInventory);
            SyncBagData(jobs, _inventory.SecondaryInventory);

            await Job.WhenAll(jobs);
            return jobs.All(job => job.GetResult());
        }

        private void SyncBagData(in List<Job<bool>> jobs, Bag bag)
        {
            if (bag.IsDataSyncWithDbPending == false) return;
            foreach (var cell in bag.Cells)
            {
                if (cell.IsDataSyncWithDbPending)
                {
                    var job = CreateDatabaseJob(cell);
                    jobs.Add(job);
                    cell.MarkDataAsSyncedWithDb();
                }
            }
            bag.MarkDataAsSyncedWithDb();
        }

        private Job<bool> CreateDatabaseJob(Cell cell)
        {
            return cell.IsEmpty
                ? JobsManager.Execute(GameDatabaseProvider.SelectObject<bool>($"SELECT remove_inventory('{_characterInfoHolder.CharacterID}', '{(int)cell.BagType}', '{cell.SlotIndex}')"))
                : JobsManager.Execute(GameDatabaseProvider.SelectObject<bool>($"SELECT upsert_inventory('{_characterInfoHolder.CharacterID}', '{(int)cell.BagType}', '{cell.SlotIndex}', " +
                                                                $"'{cell.Item.UniqueID}')"));
        }
    }
}
