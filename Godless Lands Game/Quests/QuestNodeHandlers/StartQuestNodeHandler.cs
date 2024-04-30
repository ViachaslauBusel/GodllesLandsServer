using Protocol.Data.Quests.Nodes;
using Protocol.Data.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;

namespace Godless_Lands_Game.Quests.QuestNodeHandlers
{
    internal class StartQuestNodeHandler : IQuestNodeHandler
    {

        public void Init(Component component)
        {
        }

        public QuestNode Handle(Quest quest, QuestNode node)
        {
            if (node is StartQuestNode stageNode)
            {
                return quest.Data.GetNode(stageNode.FirstNodeId);
            }

            Debug.Log.Error("QuestStageNodeHandler.Handle: Node is not a StartQuestNode");
            return null;
        }
    }
}
