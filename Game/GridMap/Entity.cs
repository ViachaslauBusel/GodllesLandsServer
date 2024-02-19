using Game.Physics;
using NetworkGameEngine;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GridMap
{
    public class Entity
    {
        protected GameObject m_gameObject;
        protected Location m_location;
        protected Tile m_tile = null;

        public int GameObjectID => m_gameObject.ID;
        public bool IsNeedUpdate { get; private set; } = false;
        public Location Location => m_location;
        public GameObject GameObject => m_gameObject;

        public Entity(GameObject gameObject)
        {
            m_gameObject = gameObject;
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
