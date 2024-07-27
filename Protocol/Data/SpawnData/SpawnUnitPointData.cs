using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.Data.SpawnData
{
    [Serializable]
    public class SpawnUnitPointData
    {
        public int UnitID;
        public UnitType UnitType;
        public SpawnPointType SpawnType;
        public Vector3 Position;
        public float SpawnRadius;
        public int MinSpawnTime;
        public int MaxSpawnTime;
    }
}
