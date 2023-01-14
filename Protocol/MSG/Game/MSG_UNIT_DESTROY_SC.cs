using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_UNIT_DESTROY, Channel.Reliable)]
    public struct MSG_UNIT_DESTROY_SC
    {
        public int UnitID { get; set; }
    }
}
