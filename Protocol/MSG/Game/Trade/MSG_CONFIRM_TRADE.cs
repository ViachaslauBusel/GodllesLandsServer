using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Trade
{
    [MessagePack(Opcode.MSG_CONFIRM_TRADE, Channel.Reliable)]
    public struct MSG_CONFIRM_TRADE 
    {
    }
}
