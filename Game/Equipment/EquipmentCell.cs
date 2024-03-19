using Game.DataSync;
using Game.Items;
using Protocol.MSG.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Equipment
{
    public class EquipmentCell : ClientAndDbSyncElement
    {
        private readonly EquipmentType _equipmentType;
        private Item _item;

        public bool IsEmpty => _item == null;
        public EquipmentType EquipmentType => _equipmentType;
        public Item Item => _item;

        public EquipmentCell(EquipmentType equipmentType)
        {
            _equipmentType = equipmentType;
        }

        internal void EquipItem(Item item)
        {
            _item = item;
            SetDataSyncPending();
        }

        internal Item TakeItem()
        {
            Item temp = _item;
            _item = null;
            SetDataSyncPending();
            return temp;
        }
    }
}
