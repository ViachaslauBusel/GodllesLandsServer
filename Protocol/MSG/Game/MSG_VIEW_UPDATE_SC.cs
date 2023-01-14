using Protocol.Data;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_HP_VIEW_UPDATE, Channel.Reliable)]
    public struct MSG_VIEW_UPDATE_SC
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public int Stamina { get; set; }
        public int MaxStamina { get; set; }
    }
}
