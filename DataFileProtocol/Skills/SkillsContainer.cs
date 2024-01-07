using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileProtocol.Skills
{
    [System.Serializable]
    public class SkillsContainer
    {
        public List<SkillData> _skills = new List<SkillData>();
        public List<MelleSkillData> _melleSkills = new List<MelleSkillData>();

        public int Count { get { return _skills.Count + _melleSkills.Count; } }

        public SkillData this[int index] { 
            get 
            { 
                if(index < _skills.Count) return _skills[index];
                index -= _skills.Count;

                if(index < _melleSkills.Count) return _melleSkills[index];

                return null;
            }
        }

        public void Add(SkillData skill)
        {
            switch (skill)
            {
                case MelleSkillData:
                    _melleSkills.Add((MelleSkillData)skill);
                    break;
                default:
                    _skills.Add(skill);
                    break;
            }
        }
        public SkillData GetSkill(int id) 
        {
            foreach (var skill in _skills)
            {
                if (skill.id == id) return skill;
            }
            foreach (var skill in _melleSkills)
            {
                if (skill.id == id) return skill;
            }
            return null;
        }
    }
}
