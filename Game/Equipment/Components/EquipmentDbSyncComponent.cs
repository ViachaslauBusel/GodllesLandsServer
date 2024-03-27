using Database;
using Game.DB;
using Game.Inventory.Components;
using Game.Items;
using Game.Items.Components;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using Protocol.MSG.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Equipment.Components
{
    internal class EquipmentDbSyncComponent : Component, IDatabaseReadable, IDatabaseWritable
    {
        struct EquipmentDbData
        {
            public int slot_id;
            public int item_uid;
        }
        private EquipmentComponent _equipment;
        private DBControlComponent _dbControl;
        private CharacterInfoHolder _characterInfoHolder;
        private ItemStorageComponent _itemStorage;
        private List<EquipmentDbData> _items;

        public DatabaseSavePriority DatabaseSavePriority => DatabaseSavePriority.Hight;

        public bool HasDataToSave => _equipment.IsDataSyncWithDbPending;

        public override void Init()
        {
            _equipment = GetComponent<EquipmentComponent>();
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
                if(itemData.item_uid == 0)
                {
                    continue;
                }
                Item item = _itemStorage.GetItem(itemData.item_uid);
                if(item == null)
                {
                    Debug.Log.Error($"Item:{itemData.item_uid} cannot be found in the storage");
                    continue;
                }
                _equipment.Equip((EquipmentType)itemData.slot_id, item);
            }
            _equipment.MarkDataAsSyncedWithDb();
        }

        public async Job ReadFromDatabaseAsync()
        {
            _items = await JobsManager.Execute(GameDatabaseProvider.Select<List<EquipmentDbData>>($"SELECT load_equipment('{_characterInfoHolder.CharacterID}')"));
        }

        public async Job<bool> WriteToDatabase()
        {
            List<Job<bool>> jobs = new List<Job<bool>>();
            foreach (var equipmentSlot in _equipment.Items)
            {
                if (equipmentSlot.IsDataSyncWithDbPending == false) continue;

                jobs.Add(
                JobsManager.Execute(GameDatabaseProvider.Call($"CALL save_equipment('{_characterInfoHolder.CharacterID}', {(int)equipmentSlot.EquipmentType}, {equipmentSlot.Item?.UniqueID ?? 0})")));
            }
            await Job.WhenAll(jobs);
            return jobs.All(j => j.GetResult());
        }
    }
}
