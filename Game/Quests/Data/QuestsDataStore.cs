using DataFileProtocol.Skills;
using Game.Resources;
using NetworkGameEngine.Debugger;
using Newtonsoft.Json;
using Protocol.Data.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Quests.Data
{
    internal class QuestsDataStore
    {
        private static Dictionary<int, QuestData> _quests = new Dictionary<int, QuestData>();
        public static void Load()
        {
            string fullPath = Path.Combine(ResourceFile.Folder, ResourceFile.Quests);
            if (File.Exists(fullPath))
            {
                string text = File.ReadAllText(fullPath);
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                var quests = JsonConvert.DeserializeObject<List<QuestData>>(text, settings);
                for (int i = 0; i < quests.Count; i++)
                {
                    QuestData quest = quests[i];
                    if (quest == null)
                    {
                        Debug.Log.Error("SkillsDataStore", "Skill with index {0} is null", i);
                        continue;
                    }
                    if (_quests.ContainsKey(quest.ID))
                    {
                        Debug.Log.Error("SkillsDataStore", "Skill with id {0} already exists", quest.ID);
                        continue;
                    }
                    _quests.Add(quest.ID, quest);
                }
            }
            else Debug.Log.Fatal("SkillsDataStore", "SkillsData.dat not found");
        }

        internal static QuestData GetData(int questId)
        {
            if (_quests.ContainsKey(questId) == false)
            {
                return null;
            }

            return _quests[questId];
        }
    }
}
