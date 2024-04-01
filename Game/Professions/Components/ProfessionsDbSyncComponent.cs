using Database;
using Game.DB;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using Protocol.MSG.Game.Professions;

namespace Godless_Lands_Game.Professions.Components
{
    public struct ProfessionDbData
    {
        public ProfessionType profession_type;
        public int level;
        public int experience;
    }
    public class ProfessionsDbSyncComponent : Component, IDatabaseReadable, IDatabaseWritable
    {
        private CharacterInfoHolder _characterInfoHolder;
        private ProfessionsComponent _professions;

        public DatabaseSavePriority DatabaseSavePriority => DatabaseSavePriority.Hight;

        public bool HasDataToSave => _professions.Professions.Any(p => p.IsDataSyncWithDbPending);


        public override void Init()
        {
            _professions = GetComponent<ProfessionsComponent>();
            _characterInfoHolder = GetComponent<CharacterInfoHolder>();
        }

        public async Job ReadFromDatabaseAsync()
        {
            ProfessionDbData[] professionDbDatas = await JobsManager.Execute(GameDatabaseProvider.Select<ProfessionDbData[]>($"SELECT load_professions('{_characterInfoHolder.CharacterID}')"));

            if(professionDbDatas == null)
            {
                return;
            }

            foreach (ProfessionDbData professionDbData in professionDbDatas)
            {
                if (_professions.TryGetProfession(professionDbData.profession_type, out Profession profession))
                {
                    profession.LoadDataFromDb(professionDbData.level, professionDbData.experience);
                }
            }
        }

        public async Job<bool> WriteToDatabase()
        {
            try
            {
                foreach (Profession profession in _professions.Professions)
                {
                    if (profession.IsDataSyncWithDbPending)
                    {
                        await JobsManager.Execute(GameDatabaseProvider.Call($"CALL save_profession('{_characterInfoHolder.CharacterID}', {(int)profession.ProfessionType}, {profession.Level}, {profession.Experience})"));
                        profession.MarkDataAsSyncedWithDb();
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
