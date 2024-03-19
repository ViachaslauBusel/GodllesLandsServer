using Protocol.Data.Items.Network;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Equipment.MSG
{
    [MessagePack(Opcode.MSG_EQUIPMENT_SYNC, Channel.Queue)]
    public struct MSG_EQUIPMENT_SYNC_SC
    {
        public List<EquipmentItemSyncData> Items { get; set;}
    }
}
