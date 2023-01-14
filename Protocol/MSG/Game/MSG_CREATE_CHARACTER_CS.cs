using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_CREATE_CHARACTER, Channel.Reliable)]
    public struct MSG_CREATE_CHARACTER_CS
    {
        public string Name { get; set; }
    }
}
