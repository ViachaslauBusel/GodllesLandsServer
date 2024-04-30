using Game.Inventory.Components;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Quests;
using Protocol.Data.Quests.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Quests.QuestNodeHandlers
{
    internal class InventoryItemAvailabilityQuestNodeHandler : IQuestNodeHandler
    {
        private InventoryComponent _inventory;

        public void Init(Component component)
        {
            _inventory = component.GetComponent<InventoryComponent>();
        }

        public QuestNode Handle(Quest quest, QuestNode node)
        {
            if (node is InventoryItemAvailabilityQuestNode inventoryItemAvailabilityNode)
            {
                int itemId = inventoryItemAvailabilityNode.ItemId;
                int amount = inventoryItemAvailabilityNode.Amount;

                if (_inventory.ContainsItem(itemId, amount))
                {
                    return quest.Data.GetNode(inventoryItemAvailabilityNode.NextNodeId);
                }
            }

            Debug.Log.Error("InventoryItemAvailabilityQuestNodeHandler.Handle: Node is not a InventoryItemAvailabilityQuestNode");
            return null;
        }
    }
}
