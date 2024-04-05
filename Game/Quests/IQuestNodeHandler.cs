using Godless_Lands_Game.Quests.Components;
using NetworkGameEngine;
using Protocol.Data.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Quests
{
    public interface IQuestNodeHandler
    {
        void Init(Component component);
        QuestNode Handle(Quest quest, QuestNode node);
    }
}
