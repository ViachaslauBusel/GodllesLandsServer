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
            _dropList.Add(_itemsFactory.CreateItem(1, count: RandomHelper.Range(1, 5)));
            _dropList.Add(_itemsFactory.CreateItem(3, count: 1));
        }

        internal List<Item> TakeAll()
        {
            List<Item> items = new List<Item>(_dropList);
            _dropList.Clear();
            OnUpdateDropList?.Invoke();
            return items;
        }

        internal void AddItems(List<Item> items)
        {
            _dropList.AddRange(items);
            OnUpdateDropList?.Invoke();
        }
    }
}
