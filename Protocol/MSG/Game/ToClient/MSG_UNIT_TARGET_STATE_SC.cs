using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.ToClient
{
    [MessagePack(Opcode.MSG_UNIT_TARGET_STATE_SC, Channel.Discard)]
    public struct MSG_UNIT_TARGET_STATE_SC
    {
        public string TargetName { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
    }
}
