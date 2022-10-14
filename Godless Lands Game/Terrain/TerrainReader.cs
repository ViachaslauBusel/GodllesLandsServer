using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Godless_Lands_Game.Terrain
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
        public double TileSize { get; private set; }
        private TerrainData[,] terrainData;



        public void Load(string file)
        {
            try {
                string fileName = Path.Combine(Environment.CurrentDirectory, file);
                if (File.Exists(fileName))
                {

                        using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                        {


                            MapSize = reader.ReadInt32();
                            BlockCount = reader.ReadInt32();
                            int maxTile = MapSize * BlockCount;
                            TileSize = (MapSize * 1_000.0) / maxTile;
                            terrainData = new TerrainData[maxTile, maxTile];

                            for (int y = 0; y < maxTile; y++)
                            {
                                for (int x = 0; x < maxTile; x++)
                                {
                                    terrainData[x, y] = new TerrainData();
                                    int sizeData = reader.ReadInt32();
                                terrainData[x, y].Data = reader.ReadBytes(sizeData);
                                }
                            }
                        }
                    
                }
                else Console.WriteLine("File terrain.dat no found");
            } 
            catch (Exception)
            {
                Console.WriteLine("File terrain.dat failed to load");
            }
        }

        public void Dispose()
        {
            terrainData = null;
        }
        public byte[] GetHeightMap(int x, int y)
        {
            return terrainData[x, y].Data;
        }

    }
}
