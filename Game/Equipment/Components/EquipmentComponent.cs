using Game.DataSync;
using Game.Items;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.MSG.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Equipment.Components
{
    public class EquipmentComponent : Component
    {
        private Dictionary<EquipmentType, EquipmentCell> _equipment = new Dictionary<EquipmentType, EquipmentCell>();
        private ClientAndDbSyncElementObject _syncElement = new ClientAndDbSyncElementObject();

        public bool IsDataSyncWithClientPending => _syncElement.IsDataSyncWithClientPending;
        public bool IsDataSyncWithDbPending => _syncElement.IsDataSyncWithDbPending;
        public IEnumerable<EquipmentCell> Items => _equipment.Values;

        public EquipmentComponent()
        {
            foreach (EquipmentType equipType in Enum.GetValues(typeof(EquipmentType)))
            {
                _equipment.Add(equipType, new EquipmentCell(equipType));
            }
        }

        internal void Equip(EquipmentType equipType, Item item)
        {
            if (_equipment.ContainsKey(equipType) == false)
            {
                Debug.Log.Error("No item in this slot");
                return;
            }

            if (_equipment[equipType].IsEmpty == false)
            {
                Debug.Log.Error("Slot is not empty");
                return;
            }

            _syncElement.SetDataSyncPending();
            _equipment[equipType].EquipItem(item);
        }

        internal Item Take(EquipmentType equipType)
        {
            if (_equipment.ContainsKey(equipType) == false)
            {
                Debug.Log.Error("No item in this slot");
                return null;
            }

            _syncElement.SetDataSyncPending();
            return _equipment[equipType].TakeItem();
        }

        public void MarkDataAsSyncedWithClient() => _syncElement.MarkDataAsSyncedWithClient();
        public void MarkDataAsSyncedWithDb() => _syncElement.MarkDataAsSyncedWithDb();
        
    }
}
