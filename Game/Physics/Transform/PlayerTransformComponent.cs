using Database;
using Game.DB;
using Game.Tools;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using System.Numerics;

namespace Game.Physics.Transform
{
    public class PlayerTransformComponent : TransformComponent, IDatabaseReadable, IDatabaseWritable
    {
        private CharacterInfoHolder m_characterInfoHolder;
        private byte _lastSaveVersion;

        public override void Init()
        {
            m_characterInfoHolder = GetComponent<CharacterInfoHolder>();
        }

        public DatabaseSavePriority DatabaseSavePriority => DatabaseSavePriority.Medium;
        public bool HasDataToSave => m_version != _lastSaveVersion;


        public async Job ReadFromDatabaseAsync()
        {
            m_position = await JobsManager.Execute(GameDatabaseProvider.Select<Vector3>($"SELECT get_chatacer_position('{m_characterInfoHolder.CharacterID}')"));
            _lastSaveVersion = ++m_version;
        }

        public async Job<bool> WriteToDatabase()
        {
            _lastSaveVersion = m_version;
            return await JobsManager.Execute(GameDatabaseProvider.Call($"CALL set_character_position('{m_characterInfoHolder.CharacterID}', '{FloatHelper.FloatToString(m_position.X)}', '{FloatHelper.FloatToString(m_position.Y)}', '{FloatHelper.FloatToString(m_position.Z)}')"));
        }
    }
}
