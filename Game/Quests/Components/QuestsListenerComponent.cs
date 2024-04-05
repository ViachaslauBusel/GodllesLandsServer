using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol;
using Protocol.Data.Quests.Nodes;
using Protocol.Data.Quests;
using Protocol.MSG.Game.Quests;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkGameEngine.Debugger;

namespace Godless_Lands_Game.Quests.Components
{
    public class QuestsListenerComponent : Component
    {
        private QuestControllerComponent _questController;
        private NetworkTransmissionComponent _networkTransmission;
        private QuestNodeHandlerStorageComponent _questNodeHandlerStorage;          

        public override void Init()
        {
            _questController = GetComponent<QuestControllerComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _questNodeHandlerStorage = GetComponent<QuestNodeHandlerStorageComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_QUEST_STAGE_UP_REQUEST, OnQuestStageUpRequest);
        }

        private void OnQuestStageUpRequest(Packet packet)
        {
            packet.Read(out MSG_QUEST_STAGE_UP_REQUEST_CS request);

            MSG_QUEST_STAGE_UP_RESPONSE_SC response = new MSG_QUEST_STAGE_UP_RESPONSE_SC();
            response.QuestID = request.QuestID;

            if (_questController.TryGetQuest(request.QuestID, out Quest quest))
            {
                response.Result = StageUp(quest);
            }
            else
            {
                response.Result = false;
            }

            _networkTransmission.Socket.Send(response);
        }

        private bool StageUp(Quest quest)
        {
            QuestNode currentNode = quest.CurrentStageID == 0 ? quest.Data.GetStartNode() : quest.Data.GetNode(quest.CurrentStageID);

            if (currentNode == null)
            {
                Debug.Log.Error($"Quest {quest.ID} has no current stage.");
                return false;
            }

            QuestNode nextNode = currentNode;
            do
            {
                IQuestNodeHandler nodeHandler = _questNodeHandlerStorage.GetHandler(nextNode);
                nextNode = nodeHandler != null ? nodeHandler.Handle(quest, nextNode) : null;

                if (nextNode is QuestStageNode)
                {
                    quest.SetCurrentStage(nextNode.ID);
                    return true;
                }
            }
            while (nextNode != null);

            return false;
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_QUEST_STAGE_UP_REQUEST);
        }
    }
}
