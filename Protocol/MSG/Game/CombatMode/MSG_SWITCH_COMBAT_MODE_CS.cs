using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.CombatMode
{
    [MessagePack(Opcode.MSG_SWITCH_COMBAT_MODE, Channel.Queue)]
    public struct MSG_SWITCH_COMBAT_MODE_CS
    {
        public bool CombatMode { get; set;}
    }
}
