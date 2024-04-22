using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Trade
{
    [MessagePack(Opcode.MSG_TRADE_REQUEST_RESPONSE, RUCP.Channel.Reliable)]
    public struct MSG_TRADE_REQUEST_RESPONSE_CS
    {
        public bool Result { get; set; }
    }
}
