using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_GAME_AUTHORIZATION, Channel.Reliable)]
    public struct MSG_GAME_AUTHORIZATION_CS
    {
        public int LoginID { get; set; }
        public int SessionToken { get; set; }
    }
}
