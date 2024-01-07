using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Hotbar
{
    [MessagePack(Opcode.MSG_HOTBAR_SET_CELL_VALUE, Channel.Queue)]
    public struct MSG_HOTBAR_SET_CELL_VALUE_CS
    {
        public byte CellIndex { get; set; }
        public HotbarCellType CellType { get; set; }
        public short CellValue { get; set; }
    }
}
