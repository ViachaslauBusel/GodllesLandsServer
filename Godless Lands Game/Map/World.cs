using Godless_Lands_Game.Characters;
using Godless_Lands_Game.Physics;
using Godless_Lands_Game.Terrain;
using RUCP;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Godless_Lands_Game.Map
{
    class World
    {
       
        public static int MaxTiles { get; private set; }
        public const double tileSize = 100.0;
        private static Tile[,] tiles;

        public static int MaxWorlds { get; private set; }
        public const double wordSize = 500.0;
        private static int tileOnWorld;
      //  private static DiscreteDynamicsWorld[,] worlds;

        private static ConcurrentDictionary<int, Character> players;
        private static TerrainReader terrainReader;

        public static void Load()
        {
            terrainReader = new TerrainReader();
            terrainReader.Load("terrain.dat");

            MaxTiles = (int)((terrainReader.MapSize * 1_000.0) / tileSize);
            tiles = MapCreator.CreateTiles(MaxTiles);

            MaxWorlds = (int)((terrainReader.MapSize * 1_000.0) / wordSize);
            tileOnWorld = MaxTiles / MaxWorlds;
           // worlds = MapCreator.CreateWorlds(terrainReader, MaxWorlds);
          
            Console.WriteLine($"tiles: {MaxTiles} worlds: {MaxWorlds} tileOnWorld: {tileOnWorld}");

            players = new ConcurrentDictionary<int, Character>();
            Thread.Sleep(130);
           // terrainReader.Dispose();
       //   Console.WriteLine($"hit { Ray.rayDown(new Vector3(50, 150, 50))}");

        }
        public static void Enter(Character character)
        {
            if(players.TryAdd(character.ID, character))
            {
                Tile.Enter(character);
            }
        }
        public static void Exit(Character character)
        {
            if (character != null && players.TryRemove(character.ID, out Character _character))
            {
                Tile.Exit(character);
            }
        }
        internal static Tile GetTile(Location location) => tiles[location.x, location.y];
        //public static DiscreteDynamicsWorld GetWorld(Location location)
        //{
        //   return worlds[location.x / tileOnWorld, location.y / tileOnWorld];
        //}
        


        internal static void TeleportToPoint(Character character, Vector3 position)
        {
            lock (character.Transform)
            {
                Tile.Exit(character);
                character.Transform.position = position;
                Tile.Enter(character);
            }
        }

        public static void SendAround(Location location, Packet packet)
        {
            foreach (Tile _map in new TileAround(location))
            {
                foreach (Character character in _map.Players)
                    character.Socket.Send(packet);
            }
        }
        public static void SendAll(Packet packet)
        {
            foreach (KeyValuePair<int, Character> character in players)
                character.Value.Socket.Send(packet);
        }
    }
}
