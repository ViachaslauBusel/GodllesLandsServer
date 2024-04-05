using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Quests
{
    [MessagePack(Opcode.MSG_QUEST_STAGE_UP_REQUEST, RUCP.Channel.Reliable)]
    public struct MSG_QUEST_STAGE_UP_REQUEST_CS
    {
        public int QuestID { get; set; }
    }
}
