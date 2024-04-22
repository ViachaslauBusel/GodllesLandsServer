using Game.Items;
using Godless_Lands_Game.ObjectInteraction.Trade.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class TradeCommandHandlerComponent : Component, IReactCommandWithResult<AddItemToTradeStorageCommand, Item>
    {
        private TraderHolderComponent _TraderHolder;


        public event Action<TradeCell[], int> OnTradeStorageChanged;


        public override void Init()
        {
            _TraderHolder = GetComponent<TraderHolderComponent>();
        }

        public Item ReactCommand(ref AddItemToTradeStorageCommand command)
        {
            Trader trader = _TraderHolder.GetTrader(command.RequesterId);

            if (trader == null)
            {
                Debug.Log.Error("Trader not found");
                return command.Item;
            }

            if(trader.IsAcceptTrade || _TraderHolder.IsTradeCancelled)
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
    }
}
