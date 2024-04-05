using Game.Items.ItemUsageLogic;
using Godless_Lands_Game.Items.ItemUsageLogic;
using Godless_Lands_Game.Quests.QuestNodeHandlers;
using NetworkGameEngine;
using Protocol.Data.Items;
using Protocol.Data.Quests;
using Protocol.Data.Quests.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Quests.Components
{
    internal class QuestNodeHandlerStorageComponent : Component
    {
        private Dictionary<Type, IQuestNodeHandler> _nodeHandlers;

        public override void Init()
        {
            _nodeHandlers = new Dictionary<Type, IQuestNodeHandler>();
            _nodeHandlers.Add(typeof(StartQuestNode), new StartQuestNodeHandler());
            _nodeHandlers.Add(typeof(NearbyNpcCheckQuestNode), new NearbyNpcCheckQuestNodeHandler());
            _nodeHandlers.Add(typeof(QuestStageNode), new QuestStageNodeHandler());
            _nodeHandlers.Add(typeof(InventoryItemAvailabilityQuestNode), new InventoryItemAvailabilityQuestNodeHandler());


            foreach (var item in _nodeHandlers.Values)
            {
                InjectDependenciesIntoObject(item);
                item.Init(this);
            }
        }

        internal IQuestNodeHandler GetHandler(QuestNode nearbyNpcCheckNode)
        {
            if (_nodeHandlers.TryGetValue(nearbyNpcCheckNode.GetType(), out IQuestNodeHandler handler))
            {
                return handler;
            }

            return null;
        }
    }
}
