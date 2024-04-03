using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Quests
{
    [MessagePack(Opcode.MSG_QUESTS_SYNC, Channel.Queue)]
    public struct MSG_QUESTS_SYNC_SC
    {
        public List<QuestSyncData> Quests { get; set; }
    }
}
