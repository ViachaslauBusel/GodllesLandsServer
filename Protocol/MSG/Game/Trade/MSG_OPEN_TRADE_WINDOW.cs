using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Trade
{
    [MessagePack(Opcode.MSG_OPEN_TRADE_WINDOW, Channel.Queue)]
    public struct MSG_OPEN_TRADE_WINDOW_SC
    {
        public bool Visible { get; set; }
        public bool PlayerLock { get; set; }
        public bool PartnerLock { get; set; }
    }
}
