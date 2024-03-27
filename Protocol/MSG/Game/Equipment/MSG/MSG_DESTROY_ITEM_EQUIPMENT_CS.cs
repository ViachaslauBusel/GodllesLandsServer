using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Equipment.MSG
{
    [MessagePack(Opcode.MSG_DESTROY_ITEM_EQUIPMENT, Channel.Reliable)]
    public struct MSG_DESTROY_ITEM_EQUIPMENT_CS
    {
        public long ItemUID { get; set; }
    }
}
