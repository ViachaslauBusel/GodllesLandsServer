using Game.Inventory.Commands;
using Game.Items;
using Godless_Lands_Game.ObjectInteraction.Trade.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.MSG.Game.Trade;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class TradeCommandHandlerComponent : Component, IReactCommandWithResult<AddItemToTradeStorageCommand, Item>, IReactCommand<ConfirmTradeCommand>
    {
        private TraderHolderComponent _traderHolder;

        public event Action<TradeCell[], int> OnTradeStorageChanged;

        public override void Init()
        {
            _traderHolder = GetComponent<TraderHolderComponent>();
        }

        public Item ReactCommand(ref AddItemToTradeStorageCommand command)
        {
            Trader trader = _traderHolder.GetTrader(command.RequesterId);

            if (trader == null)
            {
                Debug.Log.Error("Trader not found");
                return command.Item;
            }

            if (trader.IsAcceptTrade || _traderHolder.IsTradeCancelled)
            {
                Debug.Log.Error("TradeStorageComponent: ReactCommand: trade is already accepted or cancelled");
                return command.Item;
            }

            if (command.ToSlot < 0 || command.ToSlot >= trader.TradeCells.Length)
            {
                Debug.Log.Error("TradeStorageComponent: ReactCommand: command.ToSlot is out of range");
                return command.Item;
            }

            if (trader.TradeCells[command.ToSlot] != null)
            {
                int newSlot = FindEmptySlot(trader.TradeCells);
                if (newSlot == -1)
                {
                    Debug.Log.Error("TradeStorageComponent: ReactCommand: bag is full");
                    return command.Item;
                }
                command.ToSlot = newSlot;
            }

            trader.TradeCells[command.ToSlot].SetItem(command.Item);

            OnTradeStorageChanged?.Invoke(trader.TradeCells, command.RequesterId);

            return null;
        }

        private int FindEmptySlot(TradeCell[] bag)
        {
            return Array.FindIndex(bag, x => x.IsEmpty);
        }

        public void ReactCommand(ref ConfirmTradeCommand command)
        {
            Trader trader = _traderHolder.GetTrader(command.RequesterId);

            if (trader == null)
            {
                Debug.Log.Error("Trader not found");
                return;
            }

            HandleAccept(trader);
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
