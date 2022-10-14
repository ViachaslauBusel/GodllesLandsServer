using Protocol.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Login
{
    [MessagePack(Opcode.MSG_REGISTRATION_Response, RUCP.Channel.Reliable)]
    public struct MSG_REGISTRATION_Response
    {
        public LoginInformationCode Notification { get; set; }
    }
}
