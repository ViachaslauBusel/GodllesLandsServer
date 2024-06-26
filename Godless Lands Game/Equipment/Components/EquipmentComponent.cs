﻿using Game.DataSync;
using Game.Items;
using Game.UnitVisualization;
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
        private CharacterViewComponent _characterView;

        public bool IsDataSyncWithClientPending => _syncElement.IsDataSyncWithClientPending;
        public bool IsDataSyncWithDbPending => _syncElement.IsDataSyncWithDbPending;
        public IEnumerable<EquipmentCell> Items => _equipment.Values;

        public event Action OnEquipmentChanged;

        public EquipmentComponent()
        {
            foreach (EquipmentType equipType in Enum.GetValues(typeof(EquipmentType)))
            {
                _equipment.Add(equipType, new EquipmentCell(equipType));
            }
        }

        public override void Init()
        {
            _characterView = GetComponent<CharacterViewComponent>();
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
            // Update character view
            _characterView.UpdatePart(equipType, item.Data.ID);
            OnEquipmentChanged?.Invoke();
        }

        internal Item Take(EquipmentType equipType)
        {
            if (_equipment.ContainsKey(equipType) == false)
            {
                Debug.Log.Error("No item in this slot");
                return null;
            }

            _syncElement.SetDataSyncPending();
            // Update character view
            _characterView.UpdatePart(equipType, 0);
            OnEquipmentChanged?.Invoke();
            return _equipment[equipType].TakeItem();
        }

        public void MarkDataAsSyncedWithClient() => _syncElement.MarkDataAsSyncedWithClient();
        public void MarkDataAsSyncedWithDb() => _syncElement.MarkDataAsSyncedWithDb();

        internal Item GetItem(EquipmentType weaponRightHand)
        {
           if(_equipment.ContainsKey(weaponRightHand) == false)
            {
                return null;
            }
            return _equipment[weaponRightHand].Item;
        }

        internal Item TakeItem(long itemUID)
        {
            EquipmentType equipType = _equipment.Values.FirstOrDefault(x => x.Item != null && x.Item.UniqueID == itemUID)?.EquipmentType ?? EquipmentType.None;

            return Take(equipType);
        }
    }
}
