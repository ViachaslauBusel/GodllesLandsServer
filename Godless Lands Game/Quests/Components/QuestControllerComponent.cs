using Godless_Lands_Game.Quests.Data;
using NetworkGameEngine;
using Protocol.Data.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Quests.Components
{
    public class QuestControllerComponent : Component
    {
        private readonly Dictionary<int, Quest> _quests = new Dictionary<int, Quest>();

        public IReadOnlyCollection<Quest> Quests => _quests.Values;

        internal bool TryGetQuest(int quest_id, out Quest quest)
        {
            if (_quests.ContainsKey(quest_id) == false)
            {
                QuestData questData = QuestsDataStore.GetData(quest_id);
                if (questData == null)
                {
                    quest = null;
                    return false;
                }
                quest = new Quest(quest_id, questData);
                _quests.Add(quest_id, quest);
                return true;
            }

            quest = _quests[quest_id];
            return true;
        }
    }
}
