using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Professions
{
    [MessageObject]
    public struct ProfessionSyncData
    {
        public ProfessionType ProfessionType { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int ExpForLevelUp { get; set; }

        public ProfessionSyncData(ProfessionType professionType, int level, int experience, int expForLevelUp)
        {
            ProfessionType = professionType;
            Level = level;
            Experience = experience;
            ExpForLevelUp = expForLevelUp;
        }
    }
}
