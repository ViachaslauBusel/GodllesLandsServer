using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Skills
{
    [MessagePack(Opcode.MSG_SKILLS_UPDATE, RUCP.Channel.Queue)]
    public struct MSG_SKILLS_UPDATE
    {
        public List<int> Skills { get; set;}
    }
}
