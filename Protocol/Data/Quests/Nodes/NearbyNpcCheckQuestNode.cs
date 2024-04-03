using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Quests.Nodes
{
    public class NearbyNpcCheckQuestNode : QuestNode
    {
        public readonly int SuccessNodeId;
        public readonly int NpcId;

        public NearbyNpcCheckQuestNode(int id, int successNodeId, int npcId) : base(id)
        {
            SuccessNodeId = successNodeId;
            NpcId = npcId;
        }
    }
}
