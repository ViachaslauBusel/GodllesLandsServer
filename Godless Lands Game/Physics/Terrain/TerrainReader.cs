using Game.Resources;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics.Terrain
{
    class TerrainReader
    {
        /// <summary>
        /// Размер карты в КМ
        /// </summary>
        public int MapSize { get; private set; }
        /// <summary>
        /// Блоков на 1 КМ
        /// </summary>
        public int BlockCount { get; private set; }
        /// <summary>
        /// Размер тайла в метрах
        /// </summary>
        public float TileSize { get; private set; }
       // public int HeightMapResolution { get; private set; }
        private TerrainData[,] terrainData;



        public void Load(string file)
        {
            ResourceReader.Read(file, (stream) =>
            {
                MapSize = stream.ReadInt32();
                BlockCount = stream.ReadInt32();
               // HeightMapResolution = r.ReadInt32();
                int totalTiles = (int)Math.Pow(MapSize * BlockCount, 2);
                TileSize = 1_000.0f / BlockCount;
                terrainData = new TerrainData[totalTiles, totalTiles];
                for (int i = 0; i < totalTiles; i++)
                {
                    float progress = i / (float)totalTiles;
                    //   Terminal.UpdatePrint($"Reading terrain:{((progress / maxProgress) * 100.0f).ToString("0.00")}%", ConsoleColor.Yellow);
                    int x = stream.ReadInt32();
                    int y = stream.ReadInt32();
                    if (terrainData[x, y] != null) Debug.Log.Fatal("Terrain data already exists");
                    terrainData[x, y] = new TerrainData();
                    terrainData[x, y].HeightMapResolution = stream.ReadInt32();
                    int sizeData = stream.ReadInt32();
                    float[] floatArray = new float[sizeData / 4];
                    byte[] byteArray = stream.ReadBytes(sizeData);
                    Buffer.BlockCopy(byteArray, 0, floatArray, 0, byteArray.Length);
                    terrainData[x, y].Data = floatArray;
                }
                //    Terminal.PrintLine($"Reading terrain:Completed    ", ConsoleColor.Green);
            });
        }

        public void Dispose()
        {
            terrainData = null;
        }
        public TerrainData GetTerrainData(int x, int y)
        {
            return terrainData[x, y];
        }

    }
}
