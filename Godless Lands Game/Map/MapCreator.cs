
using BulletXNA;
using BulletXNA.BulletCollision;
using BulletXNA.BulletDynamics;
using BulletXNA.LinearMath;
using Godless_Lands_Game.Physics;
using Godless_Lands_Game.Terrain;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Godless_Lands_Game.Map
{
    internal class MapCreator
    {
        internal static Tile[,] CreateTiles(int maxTile)
        {


            Tile[,] map = new Tile[maxTile, maxTile];
            //  int worldOnTile = 10;//Сколько тайлов обьеденить в мир
            //    int worldTile = maxTile / worldOnTile; //Количество тайлов миров
            //  DiscreteDynamicsWorld[][] worlds = new DiscreteDynamicsWorld[worldTile][worldTile];
            for (int y = 0; y < maxTile; y++)
            {
                for (int x = 0; x < maxTile; x++)
                {
                    map[x, y] = new Tile();//createMap(terrainReader, x, y)
                }
            }
            return map;
        }

        internal static DiscreteDynamicsWorld[,] CreateWorlds(TerrainReader terrainReader, int maxWorlds)
        {
            DiscreteDynamicsWorld[,] worlds = new DiscreteDynamicsWorld[maxWorlds, maxWorlds];
            float worldSize = (float)((terrainReader.MapSize * 1_000.0) / maxWorlds);
            for (int y = 0; y < maxWorlds; y++)
            {
                for (int x = 0; x < maxWorlds; x++)
                    worlds[x, y] = CreateWorld(x * worldSize, y * worldSize, worldSize);
            }
            //Load terrain
            int maxTerrain = terrainReader.MapSize * terrainReader.BlockCount;
            int terrainOnWorld = maxTerrain / maxWorlds;
            for (int y = 0; y < maxTerrain; y++)
            {
                for (int x = 0; x < maxTerrain; x++)
                {
                    worlds[x / terrainOnWorld, y / terrainOnWorld].AddRigidBody(CreateTerrain(terrainReader, x, y));
                 //   worlds[x / terrainOnWorld, y / terrainOnWorld].StepSimulation(0.1);
                  
                }
            }
            return worlds;
        }

        public static RigidBody CreateTerrain(TerrainReader terrainReader, int x, int z)
        {
            //Загрузка террейна высота, длина, массив карты высот float, максимальная высота, направление, использовать float?
            //    HeightfieldTerrainShape m_terrainShape = new HeightfieldTerrainShape(100, 100, terrainReader.getHeightMap(x, z), 600, 1, true, false);
                 PHY_ScalarType m_type = PHY_ScalarType.PHY_FLOAT;
                 bool flipQuadEdges = false;
            // var pinned = GCHandle.Alloc(terrainReader.GetHeightMap(x, z), GCHandleType.Pinned);
           // IntPtr addres = .AddrOfPinnedObject();
       //     byte[] doubleValue = new byte[8];
           // Marshal.Copy(addres, doubleValue, 0, 8);
         //   Console.WriteLine($"test: {BitConverter.ToDouble(doubleValue)}");
                 HeightfieldTerrainShape m_terrainShape = new HeightfieldTerrainShape(129, 129, terrainReader.GetHeightMap(x, z), 1.0f,
                         0.0f, 600.0f, 1, m_type, flipQuadEdges);
            Vector3 localScaling = new Vector3(100.0f / 129.0f + 0.01f, 1.0f, 100.0f / 129.0f + 0.01f);
            m_terrainShape.SetLocalScaling(ref localScaling);
            // scale the shape
            //Vector3 localScaling = new Vector3(100.0 / 129.0 + 0.01, 1.0, 100.0 / 129.0 + 0.01);
            // m_terrainShape.SetLocalScaling (ref Vector3.One);

            // m_terrainShape.SetUseDiamondSubdivision(true);

            //CollisionObject obj = new CollisionObject()
            //{
            //  //  CollisionShape = m_terrainShape,
            //    m_worldTransform = Matrix.CreateTranslation( new Vector3((float)((x * terrainReader.TileSize) + terrainReader.TileSize / 2.0), 0.0f, (float)((z * terrainReader.TileSize) + terrainReader.TileSize / 2.0)))
            //};

            //obj.CollisionShape = m_terrainShape;

            //Matrix worldTrans = Matrix.Identity;
            //  Vector3 pos = new Vector3((x * terrainReader.TileSize) + terrainReader.TileSize/2.0, 0.0, (z * terrainReader.TileSize) + terrainReader.TileSize / 2.0);

            //      worldTrans.Origin = pos;
            //    obj.WorldTransform = worldTrans;
            var startTransform = Matrix.CreateTranslation(new Vector3((float)((x * terrainReader.TileSize) + terrainReader.TileSize / 2.0), 300.0f, (float)((z * terrainReader.TileSize) + terrainReader.TileSize / 2.0)));
            DefaultMotionState myMotionState = new DefaultMotionState(startTransform, Matrix.Identity);

            RigidBodyConstructionInfo cInfo = new RigidBodyConstructionInfo(1.0f, myMotionState, m_terrainShape, Vector3.Zero);

            RigidBody body = new RigidBody(cInfo);

            // pinned.Free();
            return body;
        }
        private static DiscreteDynamicsWorld CreateWorld(float xStart, float zStart, float worldSize)
        {
           
            // collision configuration contains default setup for memory, collision
            // setup. Advanced users can create their own configuration.
            var collisionConfiguration = new DefaultCollisionConfiguration();

            // use the default collision dispatcher. For parallel processing you
            // can use a diffent dispatcher (see Extras/BulletMultiThreaded)
            CollisionDispatcher dispatcher = new CollisionDispatcher(
                    collisionConfiguration);

            // the maximum size of the collision world. Make sure objects stay
            // within these boundaries
            // Don't make the world AABB size too large, it will harm simulation
            // quality and performance
            Vector3 worldAabbMin = new Vector3(xStart - 100.0f, -800f, zStart-100.0f);
            Vector3 worldAabbMax = new Vector3(xStart + worldSize + 100.0f, 800.0f, zStart + worldSize + 100.0f);
        //    Console.WriteLine($"Min: {worldAabbMin} Max: {worldAabbMax}");

            //m_broadphase = new AxisSweep3Internal(ref worldMin,ref worldMax);
          var  m_broadphase = new AxisSweep3Internal(ref worldAabbMin, ref worldAabbMax, 0xfffe, 0xffff, 16384, null, false);
            //  int maxProxies = 1024;


            // the default constraint solver. For parallel processing you can use a
            // different solver (see Extras/BulletMultiThreaded)
            SequentialImpulseConstraintSolver solver = new SequentialImpulseConstraintSolver();

            DiscreteDynamicsWorld dynamicsWorld = new DiscreteDynamicsWorld(
                    dispatcher, m_broadphase, solver,
                    collisionConfiguration);

            dynamicsWorld.SetGravity(new Vector3(0, 0, 0));

           
         
            return dynamicsWorld;
        }

       
    }
}