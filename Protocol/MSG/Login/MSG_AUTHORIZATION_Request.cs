using RUCP;
using System;
using System.Numerics;

namespace Protocol.MSG.Login
{
    [MessagePack(Opcode.MSG_AUTHORIZATION_Request, Channel.Reliable)]
    public partial struct MSG_AUTHORIZATION_Request
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public short Version{ get; set; }
    }
}
