using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.ToClient.Target
{
    [MessagePack(Opcode.MSG_UNIT_TARGET_FULL_SC, Channel.Discard)]
    public struct MSG_UNIT_TARGET_FULL_SC
    {
        public string TargetName { get; set; }
        public int PercentHP { get; set; }
    }
}
