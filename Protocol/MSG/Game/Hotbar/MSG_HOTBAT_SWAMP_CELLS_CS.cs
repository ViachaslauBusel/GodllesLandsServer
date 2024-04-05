using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Hotbar
{
    [MessagePack(Opcode.MSG_HOTBAT_SWAMP_CELLS, Channel.Reliable)]
    public struct MSG_HOTBAT_SWAMP_CELLS_CS
    {
        public byte FromCellIndex { get; set; }
        public byte ToCellIndex { get; set; }
    }
}
