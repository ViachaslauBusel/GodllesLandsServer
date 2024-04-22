using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Trade
{
    [MessagePack(Opcode.MSG_TRADE_REQUEST, RUCP.Channel.Reliable)]
    public struct MSG_TRADE_REQUEST_CS
    {
        public int TargetID { get; set; }
    }
}
