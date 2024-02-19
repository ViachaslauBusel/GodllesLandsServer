using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.Data.Monsters
{
    [System.Serializable]
    public class MonsterData
    {
        public int SkinID;
        public string Name;
        public int HP;
        public Vector3 SpawnPosition;
        public float SpawnRadius;
    }
}
