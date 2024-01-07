using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Messenger
{
    [MessagePack(Opcode.MSG_MESSAGE_SC, RUCP.Channel.Reliable)]
    public struct MSG_MESSAGE_SC
    {
        public MsgLayer Layer { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
    }
}
