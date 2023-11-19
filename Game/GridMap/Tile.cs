using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GridMap
{
    internal class Tile
    {
        private List<GameObject> m_gameObjects = new List<GameObject>();

        public IEnumerable<GameObject> Objects => m_gameObjects;

        internal void Add(GameObject gameObject)
        {
            m_gameObjects.Add(gameObject);
        }

        internal void Remove(GameObject gameObject)
        {
           m_gameObjects.Remove(gameObject);
        }
    }
}
