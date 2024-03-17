using Game.Items;
using Game.Items.Components;
using Protocol.MSG.Game.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Inventory
{
    public class Bag
    {
        private Cell[] _cells;
        private ItemStorageComponent _itemStorage;
        private InventoryBagType _bagId;
        private int _maxItemsCount;
        private int _maxWeight;
        private int _currentWeight;
        private int _currentItemsCount;
        private bool _isDataSyncWithClientPending;
        private bool _isDataSyncWithDbPending;

        public bool IsDataSyncWithClientPending => _isDataSyncWithClientPending;
        public bool IsDataSyncWithDbPending => _isDataSyncWithDbPending;
        public IReadOnlyCollection<Cell> Cells => _cells;

        public InventoryBagType BagType => _bagId;
        public int CurrentWeight => _currentWeight;
        public int MaxWeight => _maxWeight;
        public int MaxItems => _maxItemsCount;
        public int CurrentItemsCount => _currentItemsCount;

        public Bag(ItemStorageComponent itemStorage, InventoryBagType bagId, int maxItems, int maxWeight)
        {
            _itemStorage = itemStorage;
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
            SetDataSyncPending();

            int insertIndex = GetInsertIndex(item, inIndex);

            if (insertIndex >= 0)
            {
                bool result = _cells[insertIndex].PutItem(item);
                if (result)
                {
                    _currentItemsCount++;
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
    }
}
