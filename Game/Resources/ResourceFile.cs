using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Resources
{
    public class ResourceFile
    {
        public static string Folder { get; } = "Data";
        public static string Terrain { get; } = "terrain.dat";
        public static string Resources { get; } = "resources.dat";
        public static string Collision { get; } = "collision.dat";
        public static string Skills { get; } = "skills.dat";
        public static string Effects { get; } = "effects.dat";
        public static string Items { get; } = "items.dat";
        public static string Monsters { get; } = "monsters.dat";
        public static string MonsterSpawnPoint { get; } = "monsterSpawnPoint.dat";
        public static string Recipes { get; } = "recipes.dat";
        public static string Buildings { get; } = "buildings.dat";
        public static string NPCs { get; } = "NPCs.dat";
        public static string Quests { get; } = "quests.dat";
        public static string Bestiary { get; } = "bestiary.dat";
        public static string TerrainCollision { get; } = "terrainMeshColliders.dat";
        public static string RespawnPoints { get; } = "spawnPoints.dat";
        public static string MiningStones { get; } = "miningStones.dat";
    }
}