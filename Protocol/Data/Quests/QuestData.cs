using Protocol.Data.Quests.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Data.Quests
{
    public class QuestData
    {
        private List<QuestNode> _nodes = new List<QuestNode>();

        public readonly int ID;

        public IReadOnlyCollection<QuestNode> Nodes => _nodes;

        public QuestNode GetStartNode()
        {
            return _nodes.FirstOrDefault(n => n is StartQuestNode);
        }

        public QuestNode GetNode(int currentStageID)
        {
            return _nodes.FirstOrDefault(n => n.ID == currentStageID);
        }

        public QuestData(int id, List<QuestNode> nodes)
        {
            ID = id;
            _nodes = nodes;
        }
    }
}
