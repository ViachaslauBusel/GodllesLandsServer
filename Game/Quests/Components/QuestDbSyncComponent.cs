using Database;
using Game.DB;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;

namespace Godless_Lands_Game.Quests.Components
{
    public struct QuestDbData
    {
        public int quest_id;
        public int stage_id;
    }

    public class QuestDbSyncComponent : Component
    {
        private CharacterInfoHolder _characterInfoHolder;
        private QuestControllerComponent _questController;

        public DatabaseSavePriority DatabaseSavePriority => DatabaseSavePriority.Hight;

        public bool HasDataToSave => _questController.Quests.Any(p => p.IsDataSyncWithDbPending);


        public override void Init()
        {
            _questController = GetComponent<QuestControllerComponent>();
            _characterInfoHolder = GetComponent<CharacterInfoHolder>();
        }

        public async Job ReadFromDatabaseAsync()
        {
            QuestDbData[] questDbDatas = await JobsManager.Execute(GameDatabaseProvider.Select<QuestDbData[]>($"SELECT load_quests('{_characterInfoHolder.CharacterID}')"));

            if (questDbDatas == null)
            {
                return;
            }

            foreach (QuestDbData questDbData in questDbDatas)
            {
                if (_questController.TryGetQuest(questDbData.quest_id, out Quest quest))
                {
                    quest.SetCurrentStage(questDbData.stage_id);
                    quest.MarkDataAsSyncedWithDb();
                }
                else Debug.Log.Error($"Quest with id {questDbData.quest_id} not found in the quest controller");
            }
        }

        public async Job<bool> WriteToDatabase()
        {
            try
            {
                foreach (Quest quest in _questController.Quests)
                {
                    if (quest.IsDataSyncWithDbPending)
                    {
                        await JobsManager.Execute(GameDatabaseProvider.Call($"CALL save_quest('{_characterInfoHolder.CharacterID}', {(int)quest.ID}, {quest.CurrentStageID})"));
                        quest.MarkDataAsSyncedWithDb();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log.Error(e);
                return false;
            }
            return true;
        }
    }
}
