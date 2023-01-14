using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_SELECT_CHARACTER, Channel.Reliable)]
    public struct MSG_SELECT_CHARACTER_CS
    {
        public int CharacterID { get; set; }
    }
}
