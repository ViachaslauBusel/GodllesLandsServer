using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics.Terrain
{
    internal class TerrainData
    {
        public float[] Data { get; internal set; }
        public int HeightMapResolution { get; internal set; }
        //   public GCHandle pinnedTerrainData;
    }
}
