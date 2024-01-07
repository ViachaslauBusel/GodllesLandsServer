using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Skills
{
    [MessagePack(Opcode.MSG_SKILL_USE, RUCP.Channel.Queue)]
    public struct MSG_SKILL_USE_CS
    {
        public int SkillID { get; set; }
    }
}
