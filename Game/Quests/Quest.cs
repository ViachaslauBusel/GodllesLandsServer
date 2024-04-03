using Game.DataSync;
using Protocol.Data.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Quests
{
    public class Quest : ClientAndDbSyncElement
    {
        private readonly int _id;
        private readonly QuestData _data;
        private int _currentStageID;

        public int ID => _id;
        public QuestData Data => _data;
        public int CurrentStageID => _currentStageID;

        public Quest(int id, QuestData data)
        {
            _id = id;
            _data = data;
            SetDataSyncPendingOnlyWithClient();
        }

        public void SetCurrentStage(int stageID)
        {
            _currentStageID = stageID;
            SetDataSyncPending();
        }
    }
}
