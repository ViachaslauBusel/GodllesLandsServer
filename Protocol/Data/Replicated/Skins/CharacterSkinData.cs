﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Skins
{
    public struct CharacterSkinData : IReplicationData
    {
        public byte Version { get; set; }
        public bool InNeedChaceVisual { get; set; }

        public int WeaponId { get; set; }
        public int HeadId { get; set; }
    }
}
