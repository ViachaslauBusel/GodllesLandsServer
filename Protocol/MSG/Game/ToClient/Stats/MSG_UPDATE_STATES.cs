using Protocol.Data.Stats;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.ToClient.Stats
{
    [MessagePack(Opcode.MSG_UPDATE_STATS, Channel.Queue)]
    public struct MSG_UPDATE_STATES
    {
        public List<StatField> Stats { get; set; }
    }
}
