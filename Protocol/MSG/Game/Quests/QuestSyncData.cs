using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Quests
{
    [MessageObject]
    public struct QuestSyncData
    {
        public int QuestId { get; set; }
        public int StageID { get; set; }

        public QuestSyncData(int iD, int currentStageID) 
        {
            QuestId = iD;
            StageID = currentStageID;
        }
    }
}
