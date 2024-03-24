using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.Data.MiningStone
{
    public class ResourceMiningStoneData
    {
        public int ID { get; set; }
        public Vector3 SpawnPoint { get; set; }
        public float SpawnRadius { get; set; }
        public int StartSpawnTime { get; set; }
        public int TimeSpawn { get; set; }
        public int Profesion { get; set; }
        public int Exp { get; set; }
        public int Stamina { get; set; }
        public List<DropItemData> Drops { get; set; }
    }
}
