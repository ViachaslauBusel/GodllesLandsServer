using Game.Physics;
using NetworkGameEngine;
using RUCP;

namespace Game.GridMap
{
    public class PlayerEntity
    {
        private GameObject m_gameObject;
        private Client m_socket;
        private Location m_location;
        private Tile m_tile = null;

        public int GameObjectID => m_gameObject.ID;
        public Client Socket => m_socket;
        public bool IsNeedUpdate { get; private set; } = false;
        public Location Location => m_location;
        public GameObject GameObject => m_gameObject;

        public PlayerEntity(GameObject gameObject, Client socket)
        {
            m_gameObject = gameObject;
            m_socket = socket;
        }

     

        internal void UpdateLocation(Location location)
        {
            m_location = location;
            IsNeedUpdate = true;
        }

        internal void UpdateTile(Tile tile)
        {
            m_tile?.Remove(this);
            m_tile = tile;
            m_tile?.Add(this);
        }
    }
}
