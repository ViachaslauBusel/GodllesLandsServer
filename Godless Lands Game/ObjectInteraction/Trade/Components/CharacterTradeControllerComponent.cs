using Game.Inventory.Components;
using Game.Items;
using Game.Items.Components;
using Game.Messenger;
using Game.NetworkTransmission;
using Godless_Lands_Game.ObjectInteraction.Trade.Commands;
using NetworkGameEngine;
using NetworkGameEngine.JobsSystem;
using Protocol;
using Protocol.MSG.Game.Messenger;
using Protocol.MSG.Game.Trade;
using RUCP;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class CharacterTradeControllerComponent : Component
    {
        private NetworkTransmissionComponent _networkTransmission;
        private InventoryComponent _inventory;
        private ItemStorageComponent _itemStorage;
        private MessageBroadcastComponent _messageBroadcast;
        private GameObject _tradeObject;
        private List<long> _transferedItems;
        private bool _isTradeConfirmed = false;
        private bool _isTransferItemsInProgress = false;

        public CharacterTradeControllerComponent(GameObject tradeObject)
        {
            _tradeObject = tradeObject;
            _transferedItems = new List<long>();
        }

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _inventory = GetComponent<InventoryComponent>();
            _itemStorage = GetComponent<ItemStorageComponent>();
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_MOVE_ITEM_TO_TRADE, MoveItemToTrade);
            _networkTransmission.RegisterHandler(Opcode.MSG_CONFIRM_TRADE, ConfirmTrade);
        }

        private async void ConfirmTrade(Packet packet)
        {
            if(_isTradeConfirmed)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Trade already confirmed");
                return;
            }
            _isTradeConfirmed = true;

            if(_isTransferItemsInProgress)
            {
               await new WaitWhile(() => _isTransferItemsInProgress);
            }

            List<Job> jobs = new List<Job>();
            foreach (var itemUID in _transferedItems)
            {
                jobs.Add(_itemStorage.DestroyItem(itemUID));
            }

            await Job.WhenAll(jobs);

            _tradeObject.SendCommand(new ConfirmTradeCommand() { RequesterId = GameObject.ID });
        }

        private async void MoveItemToTrade(Packet packet)
        {
            if (_isTradeConfirmed)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Trade already confirmed");
                return;
            }

            packet.Read(out MSG_MOVE_ITEM_TO_TRADE_CS msg);

            Item item = _inventory.TakeItem(msg.ItemUID, msg.Count);
            if (item == null)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Item not found");
                return;
            }

            _isTransferItemsInProgress = true;

            long transferedItemUID = item.UniqueID;
            var cmd = new AddItemToTradeStorageCommand();
            cmd.RequesterId = GameObject.ID;
            cmd.Item = item;

            item = await _tradeObject.SendCommandAndReturnResult<AddItemToTradeStorageCommand, Item>(cmd, 1_000);

            // Return item to inventory if trade object can't accept it
            if (item != null)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, $"Trade object can't accept this item");

                _inventory.AddItem(item);
            }
            else // Items successfully transfered
            {
                _transferedItems.Add(transferedItemUID);
            }

            _isTransferItemsInProgress = false;
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_MOVE_ITEM_TO_TRADE);
            _networkTransmission.UnregisterHandler(Opcode.MSG_CONFIRM_TRADE);
        }
    }
}
