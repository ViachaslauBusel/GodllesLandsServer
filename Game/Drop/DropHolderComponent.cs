using Game.Items;
using Game.ObjectInteraction;
using Game.Tools;
using NetworkGameEngine;
using Protocol.Data.Replicated.ObjectInteraction;
using Zenject;

namespace Game.Drop
{
    public class DropHolderComponent : Component
    {
        private List<Item> _dropList = new List<Item>();
        private ItemsFactory _itemsFactory;

        public IReadOnlyList<Item> DropList => _dropList;

        public event Action OnUpdateDropList;

        [Inject]
        private void InjectServices(ItemsFactory itemsFactory)
        {
            _itemsFactory = itemsFactory;
        }

        public override void Init()
        {
            Item item = _itemsFactory.CreateItem(1, count: RandomHelper.Range(1, 5));
            item.SetOwner(1);
            _dropList.Add(item);
            item = _itemsFactory.CreateItem(3, count: 1);
            item.SetOwner(2);
            _dropList.Add(item);
        }

        public void Clear()
        {
            _dropList.Clear();
            OnUpdateDropList?.Invoke();
        }

        internal List<Item> TakeAll()
        {
            List<Item> items = new List<Item>(_dropList);
            _dropList.Clear();
            OnUpdateDropList?.Invoke();
            return items;
        }

        public void AddItem(Item item)
        {
            _dropList.Add(item);
            OnUpdateDropList?.Invoke();
        }

        internal void AddItems(List<Item> items)
        {
            _dropList.AddRange(items);
            OnUpdateDropList?.Invoke();
        }

        internal Item TakeItem(int dropIndex)
        {
            Item item = _dropList.FirstOrDefault(i => i.OwnerID == dropIndex);
            if (item != null)
            {
                _dropList.Remove(item);
                OnUpdateDropList?.Invoke();
            }
            return item;
        }
    }
}
