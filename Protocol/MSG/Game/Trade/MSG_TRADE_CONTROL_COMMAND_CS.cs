using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Trade
{
    [MessagePack(Opcode.MSG_TRADE_CONTROL_COMMAND, Channel.Reliable)]
    public struct MSG_TRADE_CONTROL_COMMAND_CS
    {
        public TradeControlCommand Command { get; set; }
    }
}
