using DataFileProtocol.Skills;
using NetworkGameEngine.Debugger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Skills.Data
{
    public static class SkillsDataStore
    {
        private static Dictionary<int, SkillData> _skills = new Dictionary<int, SkillData>();
        public static void Load()
        {
            if (File.Exists("Data/skills.dat"))
            {
                string text = File.ReadAllText("Data/skills.dat");
                SkillsContainer skills = JsonConvert.DeserializeObject<SkillsContainer>(text);
                for (int i = 0; i < skills.Count; i++)
                {
                    SkillData skill = skills[i];
                    if (skill == null)
                    {
                        Debug.Log.Error("SkillsDataStore", "Skill with index {0} is null", i);
                        continue;
                    }
                    if (_skills.ContainsKey(skill.id))
                    {
                        Debug.Log.Error("SkillsDataStore", "Skill with id {0} already exists", skill.id);
                        continue;
                    }
                    _skills.Add(skill.id, skill);
                }
            }
            else Debug.Log.Fatal("SkillsDataStore", "SkillsData.dat not found");
        }

        internal static SkillData GetData(int skillID)
        {
            if (_skills.ContainsKey(skillID) == false)
            {
                return null;
            }

            return _skills[skillID];
        }
    }
}
