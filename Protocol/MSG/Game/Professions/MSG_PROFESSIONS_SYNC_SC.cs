using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Professions
{
    [MessagePack(Opcode.MSG_PROFESSIONS_SYNC, Channel.Queue)]
    public struct MSG_PROFESSIONS_SYNC_SC
    {
        public List<ProfessionSyncData> Professions { get; set; }
    }
}
