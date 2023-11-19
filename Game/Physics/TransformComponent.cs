using Database;
using NetworkGameEngine;
using NetworkGameEngine.Units.Characters;
using Protocol.Data.Replicated;
using System.Numerics;

namespace Game.Physics
{
    public class TransformComponent : Component, IReadData<TransformData>
    {
        private byte m_version = 1;
        private Vector3 m_position;
        private float m_rotation;
        private float m_velocity;
        private bool m_inMove;
        public Vector3 Position => m_position;

        public void UpdatePosition(Vector3 position, float rotation, float velocity, bool inMove)
        {
            m_position = position;
            m_rotation = rotation;
            m_velocity = velocity;
            m_inMove = inMove;
            m_version++;
        }

        public override async Task Init()
        {
            int characterID = GetComponent<CharacterInfoHolder>().CharacterID;
            m_position = new Vector3(1180.0f, 183.0f, 1884.0f);//TODO await GameDatabaseProvider.Select<Vector3>($"SELECT get_chatacer_position('{characterID}')");
            m_version++;
        }

        public void UpdateData(ref TransformData data)
        {
            if (data.Version == m_version) return;
            data.Version = m_version;
            data.Position = m_position;
            data.Rotation = m_rotation;
            data.Velocity = m_velocity;
            data.InMove = m_inMove;
        }
    }
}
