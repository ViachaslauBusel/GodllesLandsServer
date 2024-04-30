using Game.Physics.Terrain;
using Game.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics
{
    internal class PhysicWorldCreator
    {

        internal static PhysicalTile[,] CreatePhysicalWorld(TerrainReader terrainReader, double worldSize, int maxWorlds)
        {
            PhysicalTile[,] world = new PhysicalTile[maxWorlds, maxWorlds];
            float maxProgressPhysicalTile = maxWorlds * maxWorlds;
            for (int y = 0; y < maxWorlds; y++)
            {
                for (int x = 0; x < maxWorlds; x++)
                {
                    float progressPhysicalTile = maxWorlds * y + x;
                   // Terminal.UpdatePrint($"Creating the physical world:{((progressPhysicalTile / maxProgressPhysicalTile) * 100.0f).ToString("0.00")}%", ConsoleColor.Yellow);
                    world[x, y] = new PhysicalTile();
                }
            }
           // Terminal.PrintLine($"Creating the physical world:Completed    ", ConsoleColor.Green);
            //Load terrain
            int maxTerrain = terrainReader.MapSize * terrainReader.BlockCount;
            int terrainOnWorld = maxTerrain / maxWorlds;
            float maxProgress = maxTerrain * maxTerrain;
            for (int y = 0; y < maxTerrain; y++)
            {
                for (int x = 0; x < maxTerrain; x++)
                {

                    float progress = maxTerrain * y + x;
                    //Terminal.UpdatePrint($"Loading terrain:{((progress / maxProgress) * 100.0f).ToString("0.00")}%", ConsoleColor.Yellow);
                    PhysicalTile map = world[x / terrainOnWorld, y / terrainOnWorld];
                    TerrainData terrainData = terrainReader.GetTerrainData(x, y);
                    TerrainCollider terrain = new TerrainCollider(terrainData.HeightMapResolution, terrainReader.TileSize, terrainData.Data, map.BufferPool);
                    Vector3 position = new Vector3((float)((x * terrainReader.TileSize)), 0.0f, (float)((y * terrainReader.TileSize)));//+ terrainReader.TileSize / 2.0
                    map.Add(terrain.Mesh, position, Quaternion.Identity, LayerMask.Ground);
                }
            }
            //Terminal.PrintLine($"Loading terrain:Completed    ", ConsoleColor.Green);
            return world;
        }



        //internal static void LoadCollision(string file, MeshColliders meshColliders)
        //{
        //    ResourceReader.Read(file, (r) =>
        //    {
        //        while ((r.BaseStream.Length - r.BaseStream.Position) > 0)
        //        {
        //            int colliderCount = r.ReadInt32();
        //            for (int i = 0; i < colliderCount; i++)
        //            {
        //                float x = r.ReadSingle();
        //                float y = r.ReadSingle();
        //                float z = r.ReadSingle();
        //                Vector3 position = new Vector3(x, y, z);

        //                // Debug.Log($"Object load in position:{position}");
        //                float wRotation = r.ReadSingle();
        //                float xRotation = r.ReadSingle();
        //                float yRotation = r.ReadSingle();
        //                float zRotation = r.ReadSingle();
        //                Quaternion quaternion = new Quaternion(xRotation, yRotation, zRotation, wRotation);


        //                var map = World.GetWorld(Location.CreateLocation(position));
        //                switch (r.ReadByte())
        //                {
        //                    case 1://<<<<<box <- 1
        //                        float xScale = r.ReadSingle();
        //                        float yScale = r.ReadSingle();
        //                        float zScale = r.ReadSingle();
        //                        Box box = new Box(xScale, yScale, zScale);
        //                        map.Add(box, position, quaternion, LayerMask.StaticObject);
        //                        break;
        //                    case 2://<<<<<sphere <- 2
        //                        float sphereRadius = r.ReadSingle();
        //                        Sphere sphere = new Sphere(sphereRadius);
        //                        map.Add(sphere, position, quaternion, LayerMask.StaticObject);
        //                        break;
        //                    case 3://<<<<<capsule <- 3
        //                        float capsuleRadius = r.ReadSingle();
        //                        float capsuleHeight = r.ReadSingle();
        //                        Capsule capsule = new Capsule(capsuleRadius, capsuleHeight);
        //                        map.Add(capsule, position, quaternion, LayerMask.StaticObject);
        //                        break;
        //                    case 4://<<<<<mesh <- 4
        //                        string guid = r.ReadString();
        //                        float xScaleM = r.ReadSingle();
        //                        float yScaleM = r.ReadSingle();
        //                        float zScaleM = r.ReadSingle();
        //                        //   Debug.Log($"GUID:{guid}");
        //                        if (meshColliders.Create(map.BufferPool, guid, new Vector3(xScaleM, yScaleM, zScaleM), out Mesh mesh))
        //                        {
        //                            map.Add(mesh, position, quaternion, LayerMask.StaticObject);
        //                        }
        //                        break;
        //                }
        //            }
        //        }
        //    });
        //}
    }
}
