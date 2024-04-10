using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileProtocol.Skills
{
    [System.Serializable]
    public class SkillData
    {
        public int id;
        public SkillType skillType;
        public SkillBranch branch;
        public int applyingTime;
        public int usingTime;
        public int reuseTime;
        public bool useAfter;
        public short animationId;
    }
}
