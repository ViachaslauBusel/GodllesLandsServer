using Protocol.Data;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_CHARACTERS_LIST, Channel.Reliable)]
    public struct MSG_CHARACTERS_LIST_SC
    {
        public CharacterData[] CharacterDatas { get; set; }
    }
}
