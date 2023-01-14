using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Login
{
    [MessagePack(Opcode.MSG_SERVER_LIST, RUCP.Channel.Reliable)]
    public struct MSG_SERVER_LIST_CS
    {
    }
}
