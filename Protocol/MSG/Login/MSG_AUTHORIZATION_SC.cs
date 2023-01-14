using Protocol.Data;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.MSG.Login
{
    [MessagePack(Opcode.MSG_AUTHORIZATION_Response, Channel.Reliable)]
    public struct MSG_AUTHORIZATION_SC
    {
        public int LoginID { get; set; }
        public int SessionKey { get; set; }
        public LoginInformationCode Notification { get; set; }
      
    }
}
