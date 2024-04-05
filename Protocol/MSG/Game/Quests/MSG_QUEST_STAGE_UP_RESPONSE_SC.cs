using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Quests
{
    [MessagePack(Opcode.MSG_QUEST_STAGE_UP_RESPONSE, Channel.Reliable)]
    public struct MSG_QUEST_STAGE_UP_RESPONSE_SC
    {
        public int QuestID { get; set; }
        public bool Result { get; set; }
    }
}
