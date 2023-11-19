using Game.Physics;
using NetworkGameEngine;
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
        ObjectOnMap Register(GameObject gameObject);
        void Unregister(GameObject gameObject);
        void Update();
        bool TryGetLocation(int gameObjID, out Location location);
        Tile GetTile(Location location);
    }
}
