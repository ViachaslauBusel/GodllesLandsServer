using Protocol.Data.Replicated;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.CombatMode
{
    public struct CombotModeData : IReplicationData
    {
        public bool CombatMode { get; set; }
        public byte Version { get; set; }
    }
}
