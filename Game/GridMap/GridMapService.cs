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
        private Dictionary<int, Entity> _entities = new Dictionary<int, Entity>();
        private Tile[,] _tiles;
        private ConcurrentQueue<Entity> _incomingEntities = new ConcurrentQueue<Entity>();
        private ConcurrentQueue<int> _outgoingEntities = new ConcurrentQueue<int>();

        public float TileSize { get; private set; }

        public GridMapService(float totalMapSize, float tileSize) 
        {
            TileSize = tileSize;
            int tileCount = (int)(totalMapSize / tileSize);
            _tiles = new Tile[tileCount, tileCount];

            for (int i = 0; i < tileCount; i++)
            {
                for (int j = 0; j < tileCount; j++)
                {
                    _tiles[i, j] = new Tile();
                }
            }
        }

        public void Register(Entity entity)
        {
            _incomingEntities.Enqueue(entity);
        }

        public void Unregister(int gameObjectID)
        {
            _outgoingEntities.Enqueue(gameObjectID);
        }

        public void Update()
        {
            while(_outgoingEntities.TryDequeue(out int outgoingEntityGameObjectId))
            {
                if(_entities.ContainsKey(outgoingEntityGameObjectId)) 
                {
                    _entities[outgoingEntityGameObjectId].UpdateTile(null);
                    _entities.Remove(outgoingEntityGameObjectId);
                }
            }
            while(_incomingEntities.TryDequeue(out Entity incomingEntity))
            {
                if (!_entities.TryAdd(incomingEntity.GameObjectID, incomingEntity)) { Debug.Log.Fatal($"Retrying to register an object"); }
            }

            foreach(Entity obj in _entities.Values)
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
            if (_entities.ContainsKey(gameObjID))
            {
                location = _entities[gameObjID].Location;
                return true;
            }
            return false;
        }
        public Tile GetTile(Location location)
        {
            if(location.x < 0 || location.x >= _tiles.GetLength(0)) return null;
            if(location.y < 0 || location.y >= _tiles.GetLength(1)) return null;
            return _tiles[location.x, location.y];
        }

        public IEnumerable<PlayerEntity> GetPlayersAround(int gameObjectID, int range)
        {
           if(TryGetLocation(gameObjectID, out Location location))
            {
                foreach (Location l in new LocationAround(location))
                {
                    Tile tile = GetTile(l);
                    if (tile == null) continue;
                    foreach (var reciever in tile.Entities)
                    {
                        if (reciever is PlayerEntity player)
                        { yield return player; }
                    }
                }
            }
        }

        public IEnumerable<GameObject> GetGameObjectsAround(int gameObjectID)
        {
            if (TryGetLocation(gameObjectID, out Location location))
            {
                foreach (Location l in new LocationAround(location))
                {
                    Tile tile = GetTile(l);
                    if (tile == null) continue;
                    foreach (var entity in tile.Entities)
                    {
                        yield return entity.GameObject;
                    }
                }
            }
        }
    }
}
