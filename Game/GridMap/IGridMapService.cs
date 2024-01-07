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
    internal interface IGridMapService
    {
        float TileSize { get; }
        PlayerEntity Register(GameObject gameObject, Client socket);
        void Unregister(int gameObjectID);
        void Update();
        bool TryGetLocation(int gameObjID, out Location location);
        Tile GetTile(Location location);
        IEnumerable<PlayerEntity> GetPlayersAround(int iD, int range);
    }
}
