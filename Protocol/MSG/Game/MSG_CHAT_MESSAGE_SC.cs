using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_CHAT_MESSAGE, Channel.Reliable)]
    public struct MSG_CHAT_MESSAGE_SC
    {
        public byte Layer { get; set; }
        public string Nickname { get; set; }
        public string Message { get; set; }
    }
}
