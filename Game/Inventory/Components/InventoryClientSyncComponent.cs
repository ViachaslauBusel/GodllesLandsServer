using Game.Items;
using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol.Data.Items.Network;
using Protocol.MSG.Game.Inventory;

namespace Game.Inventory.Components
{
    public class InventoryClientSyncComponent : Component
    {
        private InventoryComponent _inventory;
        private NetworkTransmissionComponent _networkTransmission;
        private List<ItemSyncData> _syncData = new List<ItemSyncData>();

        public override void Init()
        {
            _inventory = GetComponent<InventoryComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
        }

        public override void Update()
        {
            if (_inventory.PrimaryInventory.IsDataSyncWithClientPending)
            {
               SyncBagData(_inventory.PrimaryInventory);
                _inventory.PrimaryInventory.MarkDataAsSyncedWithClient();
            }
            if (_inventory.SecondaryInventory.IsDataSyncWithClientPending)
            {
               SyncBagData(_inventory.SecondaryInventory);
                _inventory.SecondaryInventory.MarkDataAsSyncedWithClient();
            }
        }

        private void SyncBagData(Bag bag)
        {
            _syncData.Clear();
            foreach (var cell in bag.Cells)
            {
                AddCellToSyncData(cell);
            }
            bag.MarkDataAsSyncedWithClient();
            // Send data to client

            MSG_INVENTORY_SYNC_SC msg = new MSG_INVENTORY_SYNC_SC
            {
                Bag = bag.BagType,
                CurrentWeight = bag.CurrentWeight,
                MaxWeight = bag.MaxWeight,
                MaxCells = bag.MaxItems,
                Items = _syncData
            };
            _networkTransmission.Socket.Send(msg);
        }

        private void AddCellToSyncData(Cell cell)
        {
            if (cell.IsDataSyncWithClientPending)
            {
                if(cell.IsEmpty)
                {
                    _syncData.Add(new ItemSyncData
                    {
                        UniqueID = 0,
                        ItemID = 0,
                        Count = 0,
                        SlotIndex = cell.SlotIndex
                    });
                }
                else
                {
                    _syncData.Add(new ItemSyncData
                    {
                        UniqueID = cell.Item.UniqueID,
                        ItemID = cell.Item.Data.ID,
                        Count = cell.Item.Count,
                        SlotIndex = cell.SlotIndex
                    });
                }
               
                cell.MarkDataAsSyncedWithClient();
                // Send data to client
            }
        }
    }
}
