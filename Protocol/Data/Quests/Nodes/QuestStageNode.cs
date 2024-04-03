using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Quests.Nodes
{
    public class QuestStageNode : QuestNode
    {
        public readonly int NextNodeId;

        public QuestStageNode(int id, int nextNodeID) : base(id)
        {
            NextNodeId = nextNodeID;
        }
    }
}
