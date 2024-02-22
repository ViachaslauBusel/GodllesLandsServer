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

        public override void Init()
        {
            m_characterInfoHolder = GetComponent<CharacterInfoHolder>();
        }

        public DatabaseSavePriority DatabaseSavePriority => DatabaseSavePriority.Medium;


        public async Job ReadFromDatabase()
        {
            m_position = await JobsManager.Execute(GameDatabaseProvider.Select<Vector3>($"SELECT get_chatacer_position('{m_characterInfoHolder.CharacterID}')"));
            m_version++;
        }

        public async Job<bool> WriteToDatabase()
        {
            return await JobsManager.Execute(GameDatabaseProvider.Call($"CALL set_character_position('{m_characterInfoHolder.CharacterID}', '{FloatHelper.FloatToString(m_position.X)}', '{FloatHelper.FloatToString(m_position.Y)}', '{FloatHelper.FloatToString(m_position.Z)}')"));
        }
    }
}
