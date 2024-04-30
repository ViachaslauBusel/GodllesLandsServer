using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Quests;
using Protocol.Data.Quests.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Quests.QuestNodeHandlers
{
    internal class QuestStageNodeHandler : IQuestNodeHandler
    {

        public void Init(Component questNodeHandlerStorageComponent)
        {
        }

        public QuestNode Handle(Quest quest, QuestNode node)
        {
            if(node is QuestStageNode stageNode)
            {
                return quest.Data.GetNode(stageNode.NextNodeId);
            }

            Debug.Log.Error("QuestStageNodeHandler.Handle: Node is not a QuestStageNode");
            return null;
        }
    }
}
