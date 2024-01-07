using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Hotbar
{
    [MessagePack(Opcode.MSG_HOTBAR_UPDATE, RUCP.Channel.Queue)]
    public struct MSG_HOTBAR_UPDATE_SC
    {
        public List<HotbarCell> Cells { get; set; }
    }
}
