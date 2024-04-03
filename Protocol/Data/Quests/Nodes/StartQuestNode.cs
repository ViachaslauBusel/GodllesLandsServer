using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Quests.Nodes
{
    public class StartQuestNode : QuestNode
    {
        public readonly int FirstNodeId;

        public StartQuestNode(int id, int firstNodeId) : base(id)
        {
            FirstNodeId = firstNodeId;
        }
    }
}
