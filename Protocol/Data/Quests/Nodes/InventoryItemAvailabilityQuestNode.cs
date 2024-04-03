using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Quests.Nodes
{
    public class InventoryItemAvailabilityQuestNode : QuestNode
    {
        public readonly int NextNodeId;
        public readonly int ItemId;
        public readonly int Amount;

        public InventoryItemAvailabilityQuestNode(int id, int nextNodeId, int itemId, int amount) : base(id)
        {
            NextNodeId = nextNodeId;
            ItemId = itemId;
            Amount = amount;
        }
    }
}
