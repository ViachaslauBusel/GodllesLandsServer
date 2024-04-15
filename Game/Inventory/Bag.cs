using Game.Items;
using Game.Items.Components;
using Godless_Lands_Game.Inventory;
using NetworkGameEngine.Debugger;
using Protocol.Data.Items;
using Protocol.MSG.Game.Inventory;

namespace Game.Inventory
{
    public class Bag
    {
        private Cell[] _cells;
        private ItemStorageComponent _itemStorage;
        private ItemsFactory _itemFactory;
        private ItemStorageType _bagId;
        private int _maxItemsCount;
        private int _maxWeight;
        private int _currentWeight;
        private int _currentItemsCount;
        private bool _isDataSyncWithClientPending;
        private bool _isDataSyncWithDbPending;

        public bool IsDataSyncWithClientPending => _isDataSyncWithClientPending;
        public bool IsDataSyncWithDbPending => _isDataSyncWithDbPending;
        public IReadOnlyCollection<Cell> Cells => _cells;

        public ItemStorageType BagType => _bagId;
        public int CurrentWeight => _currentWeight;
        public int MaxWeight => _maxWeight;
        public int MaxItems => _maxItemsCount;
        public int CurrentItemsCount => _currentItemsCount;

        public Bag(ItemStorageComponent itemStorage, ItemsFactory itemsFactory, ItemStorageType bagId, int maxItems, int maxWeight)
        {
            _itemStorage = itemStorage;
            _itemFactory = itemsFactory;
            _bagId = bagId;
            _maxItemsCount = maxItems;
            _maxWeight = maxWeight;
            _cells = InitializeCells(maxItems);
            _isDataSyncWithClientPending = true;
        }

        private Cell[] InitializeCells(int maxItems)
        {
            Cell[] cells = new Cell[maxItems];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new Cell(_itemStorage, _bagId, i);
            }
            return cells;
        }

        private int GetInsertIndex(Item item, int inIndex)
        {
            if (inIndex == -1 && item.Data.IsStackable)
            {
                // Find cell with the same item
                int cellIndex = Array.FindIndex(_cells, cell => cell.Item != null && cell.Item.Data.ID == item.Data.ID);
                if (cellIndex != -1)
                {
                    return cellIndex;
                }
            }

            int insertIndex = IsEmpyCell(inIndex) ? inIndex : FindEmptyCellIndex();

            return insertIndex;
        }

        public bool AddItem(Item item, int inIndex = -1)
        {
            if(item == null)
            {
                Debug.Log.Error("Cannot add null item to the bag");
                return false;
            }

            int insertIndex = GetInsertIndex(item, inIndex);

            if (insertIndex >= 0)
            {
                bool shouldIncreaseCount = _cells[insertIndex].IsEmpty;
                bool result = _cells[insertIndex].PutItem(item);
                if (result)
                {
                    SetDataSyncPending();
                    if(shouldIncreaseCount) _currentItemsCount++;
                    _currentWeight += item.Data.Weight * item.Count;
                }
                return result;
            }

            return false;
        }

        private bool IsEmpyCell(int index)
        {
            if (index >= 0 && index < _cells.Length)
            { return _cells[index].IsEmpty; }
            return false;
        }

        private int FindEmptyCellIndex()
        {
            return Array.FindIndex(_cells, cell => cell.IsEmpty);
        }

        private void SetDataSyncPending()
        {
            _isDataSyncWithClientPending = true;
            _isDataSyncWithDbPending = true;
        }

        internal void MarkDataAsSyncedWithClient()
        {
           _isDataSyncWithClientPending = false;
        }

        internal void MarkDataAsSyncedWithDb()
        {
            _isDataSyncWithDbPending = false;
        }

        internal bool HasItem(long itemUID)
        {
            return _cells.Any(cell => cell.Item != null && cell.Item.UniqueID == itemUID);
        }

        internal Item GetItem(long itemUID)
        {
            return _cells.FirstOrDefault(cell => cell.Item != null && cell.Item.UniqueID == itemUID)?.Item;
        }

        internal Item TakeItem(long itemUID)
        {
          int cellIndex = Array.FindIndex(_cells, cell => cell.Item != null && cell.Item.UniqueID == itemUID);
            if (cellIndex != -1)
            {
                SetDataSyncPending();
                _currentItemsCount--;
                _currentWeight -= _cells[cellIndex].Item.Data.Weight * _cells[cellIndex].Item.Count;
                return _cells[cellIndex].TakeItem();
            }
            return null;
        }

        internal void TakeItemByItemId(int itemID, int amount, in List<ItemSlot> items)
        {
            foreach(var cell in _cells)
            {
                if(cell.IsEmpty || cell.Item.Data.ID != itemID)
                {
                    continue;
                }

                if(cell.Item.Count > amount)
                {
                    Item item = _itemFactory.CreateItem(cell.Item.Data.ID, 0, amount);
                    items.Add(new ItemSlot(_bagId, cell.SlotIndex, item));
                    cell.RemoveItem(amount);
                    _currentWeight -= cell.Item.Data.Weight * amount;
                    SetDataSyncPending();
                    return;
                }
                else
                {
                    _currentItemsCount--;
                    _currentWeight -= cell.Item.Data.Weight * cell.Item.Count;
                    amount -= cell.Item.Count;
                    items.Add(new ItemSlot(_bagId, cell.SlotIndex, cell.TakeItem()));
                    SetDataSyncPending();
                }

                if(amount == 0)
                {
                    return;
                }
            }
        }

        internal bool RemoveItem(long itemUID, int count)
        {
           int itemIndex = Array.FindIndex(_cells, cell => cell.Item != null && cell.Item.UniqueID == itemUID);
            if (itemIndex != -1)
            {
                if (_cells[itemIndex].Item.Data.IsStackable && _cells[itemIndex].Item.Count < count)
                {
                   Debug.Log.Error($"Not enough items to remove from cell {itemIndex}");
                    return false;
                }
                SetDataSyncPending();
              
                _currentWeight -= _cells[itemIndex].Item.Data.Weight * Math.Min(count, _cells[itemIndex].Item.Count);
                _cells[itemIndex].RemoveItem(count);
                if (_cells[itemIndex].IsEmpty)
                {
                    _currentItemsCount--;
                }
                SetDataSyncPending();
                return true;
            }
            return false;
        }

        internal void SwampItems(long itemUID, int toCellIndex)
        {
            int fromCellIndex = Array.FindIndex(_cells, cell => cell.Item != null && cell.Item.UniqueID == itemUID);
            if (fromCellIndex == -1 || toCellIndex < 0 || toCellIndex >= _cells.Length)
            {
                Debug.Log.Error($"Cannot swamp items from cell {fromCellIndex} to cell {toCellIndex}");
                return;
            }
            Item temp = _cells[fromCellIndex].TakeItem();
            _cells[fromCellIndex].PutItem(_cells[toCellIndex].TakeItem());
            _cells[toCellIndex].PutItem(temp);
            SetDataSyncPending();
        }

        internal int GetItemCount(int itemId)
        {
            return _cells.Where(cell => cell.Item != null && cell.Item.Data.ID == itemId).Sum(cell => cell.Item.Count);
        }
    }
}
