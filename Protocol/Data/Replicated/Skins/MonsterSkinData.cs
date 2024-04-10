using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Skins
{
    public struct MonsterSkinData : IReplicationData
    {
        public int SkinID { get; set;}
        public byte Version { get; set;}
        public bool InNeedChaceVisual { get; set; }
    }
}
