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

        public IReadOnlyList<Item> DropList => _dropList;

        public event Action OnUpdateDropList;


        public override void Init()
        {
         
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
            item.SetOwner(_dropList.Count + 1);
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
