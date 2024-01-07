using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Hotbar
{
    [MessageObject]
    public struct HotbarCell
    {
        public byte CellIndex { get; set; }
        public HotbarCellType CellType { get; set; }
        public short CellValue { get; set; }
    }
}
