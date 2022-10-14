﻿using Protocol.Data;

namespace Protocol.MSG.Login
{
    [MessagePack(Opcode.MSG_SERVER_LIST, RUCP.Channel.Reliable)]
    public struct MSG_SERVER_LIST
    {
      public ServerInfo[] Servers { get; set; }
    }
}
