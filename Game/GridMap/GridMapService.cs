using Game.Physics;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GridMap
{
    public class ObjectOnMap
    {
        private GameObject m_gameObject;
        private Location m_location;
        private Tile m_tile = null;

        public int GameObjectID => m_gameObject.ID;
        public bool IsNeedUpdate { get; private set; } = false;
        public Location Location => m_location;


        public ObjectOnMap(GameObject gameObject)
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
            m_tile?.Remove(m_gameObject);
            m_tile = tile;
            m_tile?.Add(m_gameObject);
        }
    }
    internal class GridMapService : IGridMapService
    {
        private Dictionary<int, ObjectOnMap> m_objects = new Dictionary<int, ObjectOnMap>();
        private Tile[,] m_tiles;
        private ConcurrentQueue<ObjectOnMap> m_incomingObj = new ConcurrentQueue<ObjectOnMap>();
        private ConcurrentQueue<GameObject> m_outgoingObj = new ConcurrentQueue<GameObject>();

        public float TileSize { get; private set; }
        public GridMapService(float totalMapSize, float tileSize) 
        {
            TileSize = tileSize;
            int tileCount = (int)(totalMapSize / tileSize);
            m_tiles = new Tile[tileCount, tileCount];

            for (int i = 0; i < tileCount; i++)
            {
                for (int j = 0; j < tileCount; j++)
                {
                    m_tiles[i, j] = new Tile();
                }
            }
        }

        public ObjectOnMap Register(GameObject gameObject)
        {
            ObjectOnMap objectOnMap = new ObjectOnMap(gameObject);
            m_incomingObj.Enqueue(objectOnMap);
            return objectOnMap;
        }

        public void Unregister(GameObject gameObject)
        {
            m_outgoingObj.Enqueue(gameObject);
        }

        public void Update()
        {
            while(m_outgoingObj.TryDequeue(out GameObject outgoingGameObject))
            {
                if(m_objects.ContainsKey(outgoingGameObject.ID)) 
                {
                    m_objects[outgoingGameObject.ID].UpdateTile(null);
                    m_objects.Remove(outgoingGameObject.ID);
                }
            }
            while(m_incomingObj.TryDequeue(out ObjectOnMap incomingObject))
            {
                if (!m_objects.TryAdd(incomingObject.GameObjectID, incomingObject)) { Debug.Log.Fatal($"Retrying to register an object"); }
            }

            foreach(ObjectOnMap obj in m_objects.Values)
            {
                if(obj.IsNeedUpdate)
                {
                    obj.UpdateTile(GetTile(obj.Location));
                }
            }
        }
        public bool TryGetLocation(int gameObjID, out Location location)
        {
            location = new Location();
            if (m_objects.ContainsKey(gameObjID))
            {
                location = m_objects[gameObjID].Location;
                return true;
            }
            return false;
        }
        public Tile GetTile(Location location)
        {
            if(location.x < 0 || location.x >= m_tiles.GetLength(0)) return null;
            if(location.y < 0 || location.y >= m_tiles.GetLength(1)) return null;
            return m_tiles[location.x, location.y];
        }
    }
}
