using Database;
using Game.DB;
using Game.Inventory.Components;
using Game.Items;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using Zenject;

namespace Godless_Lands_Game.Items.Components
{
    internal class OfflineItemAdderComponent : Component, IDatabaseReadable
    {
        struct GivenItemDbData
        {
            public int item_id;
            public int item_count;
        }

        private InventoryComponent _inventory;
        private ItemsFactory _itemsFactory;

        [Inject]
        private void InjectServices(ItemsFactory itemsFactory)
        {
            _itemsFactory = itemsFactory;
        }

        public override void Init()
        {
            _inventory = GetComponent<InventoryComponent>();
        }

        public async Job ReadFromDatabaseAsync()
        {
            CharacterInfoHolder characterInfoHolder = GetComponent<CharacterInfoHolder>();

            GivenItemDbData[] itemDbDatas = await JobsManager.Execute(GameDatabaseProvider.Select<GivenItemDbData[]>
              ($"SELECT load_items_given_offline('{characterInfoHolder.CharacterID}')"));


            if (itemDbDatas == null) return;

            foreach (var itemData in itemDbDatas)
            {
                Item item = _itemsFactory.CreateItem(itemData.item_id, 0, itemData.item_count);
                if (item == null)
                {
                 Debug.Log.Error($"Item:{itemData.item_id} cannot be created");
                    continue;
                }
                
                if(_inventory.AddItem(item) == false)
                {
                    Debug.Log.Error($"Item:{itemData.item_id} cannot be added to inventory");
                }
            }

           GameObject.DestroyComponent(this);
        }
    }
}
