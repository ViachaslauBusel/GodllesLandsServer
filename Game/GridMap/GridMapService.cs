using Game.Physics;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using RUCP;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GridMap
{
    internal class GridMapService : IGridMapService
    {
        private Dictionary<int, PlayerEntity> m_players = new Dictionary<int, PlayerEntity>();
        private Tile[,] m_tiles;
        private ConcurrentQueue<PlayerEntity> m_incomingPlayers = new ConcurrentQueue<PlayerEntity>();
        private ConcurrentQueue<int> m_outgoingPlayers = new ConcurrentQueue<int>();

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

        public PlayerEntity Register(GameObject gameObject, Client socket)
        {
            PlayerEntity objectOnMap = new PlayerEntity(gameObject, socket);
            m_incomingPlayers.Enqueue(objectOnMap);
            return objectOnMap;
        }

        public void Unregister(int gameObjectID)
        {
            m_outgoingPlayers.Enqueue(gameObjectID);
        }

        public void Update()
        {
            while(m_outgoingPlayers.TryDequeue(out int outgoingPlayerGameObjectId))
            {
                if(m_players.ContainsKey(outgoingPlayerGameObjectId)) 
                {
                    m_players[outgoingPlayerGameObjectId].UpdateTile(null);
                    m_players.Remove(outgoingPlayerGameObjectId);
                }
            }
            while(m_incomingPlayers.TryDequeue(out PlayerEntity incomingObject))
            {
                if (!m_players.TryAdd(incomingObject.GameObjectID, incomingObject)) { Debug.Log.Fatal($"Retrying to register an object"); }
            }

            foreach(PlayerEntity obj in m_players.Values)
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
            if (m_players.ContainsKey(gameObjID))
            {
                location = m_players[gameObjID].Location;
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

        public IEnumerable<PlayerEntity> GetPlayersAround(int gameObjectID, int range)
        {
           if(TryGetLocation(gameObjectID, out Location location))
            {
                foreach (Location l in new LocationAround(location))
                {
                    Tile tile = GetTile(l);
                    if (tile == null) continue;
                    foreach (var reciever in tile.Players)
                    {
                        yield return reciever;
                    }
                }
            }
        }
    }
}
