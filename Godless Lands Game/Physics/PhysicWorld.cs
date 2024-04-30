using Game.Physics.Terrain;
using Game.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics
{
    public class PhysicWorld
    {
        private const float TILE_SIZE = 500.0f;
        private PhysicalTile[,] _tiles;
      
        public void Load()
        {
            TerrainReader terrainReader = new TerrainReader();
            terrainReader.Load(ResourceFile.Terrain);

            double size = (terrainReader.MapSize * 1_000.0f);
            int maxWorlds = (int)(size / TILE_SIZE);

            _tiles = PhysicWorldCreator.CreatePhysicalWorld(terrainReader, TILE_SIZE, maxWorlds);
        }

        internal PhysicalTile GetTile(Vector3 start)
        {
            int x = (int)(start.X / TILE_SIZE);
            int y = (int)(start.Z / TILE_SIZE);

            if (x < 0 || y < 0 || x >= _tiles.GetLength(0) || y >= _tiles.GetLength(1))
                return null;

            return _tiles[x, y];
        }
    }
}
