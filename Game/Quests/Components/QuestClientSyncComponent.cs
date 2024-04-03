using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol.MSG.Game.Professions;
using Protocol.MSG.Game.Quests;

namespace Godless_Lands_Game.Quests.Components
{
    internal class QuestClientSyncComponent : Component
    {
        private QuestControllerComponent _questController;
        private NetworkTransmissionComponent _networkTransmissionComponent;
        private List<QuestSyncData> _syncData = new List<QuestSyncData>();

        public override void Init()
        {
            _questController = GetComponent<QuestControllerComponent>();
            _networkTransmissionComponent = GetComponent<NetworkTransmissionComponent>();
        }

        public override void LateUpdate()
        {
            // Check if any quest has data that needs to be synced with the client
            if (_questController.Quests.Any(q => q.IsDataSyncWithClientPending))
            {
                foreach (Quest quest in _questController.Quests)
                {
                    // If data is pending, add it to the sync data list and mark it as synced with the client
                    if (quest.IsDataSyncWithClientPending)
                    {
                        _syncData.Add(new QuestSyncData(quest.ID, quest.CurrentStageID));
                        quest.MarkDataAsSyncedWithClient();
                    }
                }
                // Send the sync data to the client
                MSG_QUESTS_SYNC_SC msg = new MSG_QUESTS_SYNC_SC();
                msg.Quests = _syncData;
                _networkTransmissionComponent.Socket.Send(msg);

                _syncData.Clear();
            }
        }
    }
}
