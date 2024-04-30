using Game.Physics;
using NetworkGameEngine;
using RUCP;

namespace Game.GridMap
{
    public class PlayerEntity : Entity
    {
        private Client m_socket;

        public Client Socket => m_socket;

        public PlayerEntity(GameObject gameObject, Client socket) : base(gameObject)
        {
            m_socket = socket;
        }
    }
}
