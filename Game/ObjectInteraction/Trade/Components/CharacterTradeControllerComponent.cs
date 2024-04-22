using Game.Inventory.Commands;
using Game.Inventory.Components;
using Game.Items;
using Game.Messenger;
using Game.NetworkTransmission;
using Godless_Lands_Game.ObjectInteraction.Trade.Commands;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.Messenger;
using Protocol.MSG.Game.Trade;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class CharacterTradeControllerComponent : Component
    {
        private NetworkTransmissionComponent _networkTransmission;
        private InventoryComponent _inventory;
        private MessageBroadcastComponent _messageBroadcast;
        private GameObject _tradeObject;

        public CharacterTradeControllerComponent(GameObject tradeObject)
        {
            _tradeObject = tradeObject;
        }

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _inventory = GetComponent<InventoryComponent>();
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_MOVE_ITEM_TO_TRADE, MoveItemToTrade);
        }

        private async void MoveItemToTrade(Packet packet)
        {
            packet.Read(out MSG_MOVE_ITEM_TO_TRADE_CS msg);

            Item item = _inventory.TakeItem(msg.ItemUID, msg.Count);
            if (item == null)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Item not found");
                return;
            }

            var cmd = new AddItemToTradeStorageCommand();
            cmd.RequesterId = GameObject.ID;
            cmd.Item = item;

            item = await _tradeObject.SendCommandAndReturnResult<AddItemToTradeStorageCommand, Item>(cmd, 1_000);

            if (item != null)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, $"Trade object can't accept this item");

                _inventory.AddItem(item);
            }
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_MOVE_ITEM_TO_TRADE);
        }
    }
}
