using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Quests
{
    public class QuestData
    {
        private List<QuestNode> _nodes = new List<QuestNode>();

        public readonly int ID;

        public IReadOnlyCollection<QuestNode> Nodes => _nodes;

        public QuestData(int id, List<QuestNode> nodes)
        {
            ID = id;
            _nodes = nodes;
        }
    }
}
