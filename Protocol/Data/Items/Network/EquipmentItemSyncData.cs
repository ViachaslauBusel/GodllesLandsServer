using Protocol.MSG.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Items.Network
{
    [MessageObject]
    public struct EquipmentItemSyncData
    {
        public long UniqueID { get; set; }
        public int ItemID { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }
}
