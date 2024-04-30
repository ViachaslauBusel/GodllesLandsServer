using Game.Inventory.Components;
using Game.Items;
using Game.Messenger;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Quests;
using Protocol.Data.Quests.Nodes;
using Protocol.MSG.Game.Messenger;
using Zenject;

namespace Godless_Lands_Game.Quests.QuestNodeHandlers
{
    internal class ItemRewardNodeHandler : IQuestNodeHandler
    {
        private InventoryComponent _inventory;
        private ItemsFactory _itemsFactory;
        private MessageBroadcastComponent _messageBroadcast;


        [Inject]
        public void InjectServices(ItemsFactory itemsFactory)
        {
            _itemsFactory = itemsFactory;
        }

        public void Init(Component component)
        {
            _inventory = component.GetComponent<InventoryComponent>();
            _messageBroadcast = component.GetComponent<MessageBroadcastComponent>();
        }

        public QuestNode Handle(Quest quest, QuestNode node)
        {
            if (node is ItemRewardNode itemRewardNode)
            {
                Item item = _itemsFactory.CreateItem(itemRewardNode.ItemId, 0, itemRewardNode.Amount);
                _inventory.AddItem(item);

                _messageBroadcast.SendMessage(MsgLayer.System, $"You received %item_name:{item.Data.ID}:{item.Count}");

                return quest.Data.GetNode(itemRewardNode.NextNodeId);
            }

            Debug.Log.Error("InventoryItemAvailabilityQuestNodeHandler.Handle: Node is not a InventoryItemAvailabilityQuestNode");
            return null;
        }
    }
}
