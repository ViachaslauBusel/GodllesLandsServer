using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Login
{
    [MessagePack(Opcode.MSG_REGISTRATION_Request, RUCP.Channel.Reliable)]
    public struct MSG_REGISTRATION_Request
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public short Version { get; set; }
    }
}
