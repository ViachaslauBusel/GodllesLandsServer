using Game.Physics;
using NetworkGameEngine;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.GridMap
{
    public interface IGridMapService
    {
        float TileSize { get; }
        void Register(Entity entity);
        void Unregister(int gameObjectID);
        void Update();
        bool TryGetLocation(int gameObjID, out Location location);
        Tile GetTile(Location location);
        IEnumerable<PlayerEntity> GetPlayersAround(int iD, int range);
        IEnumerable<GameObject> GetGameObjectsAround(int gameObjectID);
    }
}
