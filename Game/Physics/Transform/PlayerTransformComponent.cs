using Database;
using Game.DB;
using Game.Tools;
using NetworkGameEngine.JobsManagment;
using NetworkGameEngine.Units.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
            m_position = await JobsSystem.Execute(GameDatabaseProvider.Select<Vector3>($"SELECT get_chatacer_position('{m_characterInfoHolder.CharacterID}')"));
            m_version++;
        }

        public async Job<bool> WriteToDatabase()
        {
            return await JobsSystem.Execute(GameDatabaseProvider.Call($"CALL set_character_position('{m_characterInfoHolder.CharacterID}', '{FloatHelper.FloatToString(m_position.X)}', '{FloatHelper.FloatToString(m_position.Y)}', '{FloatHelper.FloatToString(m_position.Z)}')"));
        }
    }
}
