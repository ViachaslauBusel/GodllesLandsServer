using Game.Items;
using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol.Data.Items.Network;
using Protocol.MSG.Game.Equipment.MSG;

namespace Game.Equipment.Components
{
    public class EquipmentClientSyncComponent : Component
    {
        private EquipmentComponent _equipment;
        private NetworkTransmissionComponent _networkTransmission;
        private List<EquipmentItemSyncData> _syncData = new List<EquipmentItemSyncData>();

        public override void Init()
        {
            _equipment = GetComponent<EquipmentComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
        }

        public override void Update() 
        {
            if(_equipment.IsDataSyncWithClientPending)
            {
                _syncData.Clear();
                foreach (var cell in _equipment.Items)
                {
                    if(cell.IsDataSyncWithClientPending == false) continue;
                    if(cell.IsEmpty)
                    {
                        _syncData.Add(new EquipmentItemSyncData
                        {
                            UniqueID = 0,
                            ItemID = 0,
                            EquipmentType = cell.EquipmentType
                        });
                    }
                    else
                    {
                        _syncData.Add(new EquipmentItemSyncData
                        {
                            UniqueID = cell.Item.UniqueID,
                            ItemID = cell.Item.Data.ID,
                            EquipmentType = cell.EquipmentType
                        });
                    }
                    cell.MarkDataAsSyncedWithClient();
                }

                if (_syncData.Count > 0)
                {
                    _networkTransmission.Socket.Send(new MSG_EQUIPMENT_SYNC_SC
                    {
                        Items = _syncData
                    });
                }
                _equipment.MarkDataAsSyncedWithClient();
            }
        }
    }
}
