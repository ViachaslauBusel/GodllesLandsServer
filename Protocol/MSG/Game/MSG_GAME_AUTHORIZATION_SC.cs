using Protocol.Data;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_GAME_AUTHORIZATION, Channel.Reliable)]
    public struct MSG_GAME_AUTHORIZATION_SC
    {
        public LoginInformationCode InformationCode { get; set; }
    }
}
