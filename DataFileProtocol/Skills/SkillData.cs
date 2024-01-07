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
        public float applyingTime;
        public float reuseTime;
        public bool useAfter;
        public short animationId;

       
    }
}
