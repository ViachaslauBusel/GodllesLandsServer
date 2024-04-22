using Game.Drop;
using Game.Inventory.Commands;
using Game.Items;
using Game.NetworkTransmission;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.MSG.Game.Trade;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class TradeListenerComponent : Component
    {
        private PlayersNetworkTransmissionComponent _playersNetworkTransmission;
        private TradeInteractionComponent _tradeInteraction;
        private TraderHolderComponent _traderHolder;
        private List<IPlayerNetworkProfile> _traders = new List<IPlayerNetworkProfile>();

        public override void Init()
        {
            _playersNetworkTransmission = GetComponent<PlayersNetworkTransmissionComponent>();
            _tradeInteraction = GetComponent<TradeInteractionComponent>();
            _traderHolder = GetComponent<TraderHolderComponent>();

            _tradeInteraction.OnClientConnected += OnClientConnected;
            _tradeInteraction.OnClientDisconnected += OnClientDisconnected;
        }

        private void OnClientDisconnected(GameObject @object, IPlayerNetworkProfile profile)
        {
            _traders.Remove(profile);
            _playersNetworkTransmission.UnregisterHandler(profile, Opcode.MSG_TRADE_CONTROL_COMMAND);
        }

        private void OnClientConnected(GameObject @object, IPlayerNetworkProfile profile, List<Component> list)
        {
            _traders.Add(profile);
            _playersNetworkTransmission.RegisterHandler(profile, Opcode.MSG_TRADE_CONTROL_COMMAND, OnReceiveControlCommand);
        }

        private void OnReceiveControlCommand(IPlayerNetworkProfile profile, Packet packet)
        {
            packet.Read(out MSG_TRADE_CONTROL_COMMAND_CS cmd);

            Trader trader = _traderHolder.GetTrader(profile.CharacterObjectID);

            if(trader == null)
            {
                Debug.Log.Error($"Player {profile.CharacterObjectID} is not a trader");
                return;
            }

            switch (cmd.Command)
            {
                case TradeControlCommand.Accept:
                    HandleAccept(trader);
                    break;
                case TradeControlCommand.Cancel:
                    HandleCancel(trader);
                    break;
                default:
                    Debug.Log.Error($"Unknown command {cmd.Command}");
                    break;
            }
        }

        private void HandleCancel(Trader trader)
        {
            trader.CancelTrade();
            GameObject.World.RemoveGameObject(GameObject.ID);
        }

        private void HandleAccept(Trader trader)
        {
            trader.AcceptTrade();

            if (_traderHolder.Initiator.IsAcceptTrade && _traderHolder.Acceptor.IsAcceptTrade)
            {
                TransferItems(_traderHolder.Initiator, _traderHolder.Acceptor);
                TransferItems(_traderHolder.Acceptor, _traderHolder.Initiator);
                GameObject.World.RemoveGameObject(GameObject.ID);
            }
            else
            {
                SyncWindowLock(_traderHolder.Initiator, _traderHolder.Acceptor);
                SyncWindowLock(_traderHolder.Acceptor, _traderHolder.Initiator);
            }
           
        }

        private void SyncWindowLock(Trader owner, Trader partner)
        {
            MSG_OPEN_TRADE_WINDOW_SC msg = new MSG_OPEN_TRADE_WINDOW_SC();
            msg.Visible = true;
            msg.PlayerLock = owner.IsAcceptTrade;
            msg.PartnerLock = partner.IsAcceptTrade;
            owner.Profile?.Client.Send(msg);
        }

        private void TransferItems(Trader from, Trader to)
        {
            List<Item> items = new List<Item>();
            foreach (var tradeCell in from.TradeCells)
            {
                if (!tradeCell.IsEmpty)
                {
                    items.Add(tradeCell.TakeItem());
                }
            }

            if (items.Count > 0)
            {
                AddItemToInventoryCommandNoRet addItemsCmd = new AddItemToInventoryCommandNoRet();
                addItemsCmd.Items = items;
                to.GameObject.SendCommand(addItemsCmd);
            }
        }
    }
}
