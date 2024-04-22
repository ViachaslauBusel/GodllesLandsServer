using Game.Inventory.Commands;
using Game.Items;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class TradeCancellationItemReturnComponent : Component
    {
        private TraderHolderComponent _traderHolder;


        public override void Init()
        {
            _traderHolder = GetComponent<TraderHolderComponent>();
        }

        public override void OnDestroy()
        {
            foreach (var trader in _traderHolder.Traders)
            {
                List<Item> items = new List<Item>();
                foreach (var tradeCell in trader.TradeCells)
                {
                    if (!tradeCell.IsEmpty)
                    {
                       items.Add(tradeCell.TakeItem());
                    }
                }
                if(items.Count > 0)
                {
                    AddItemToInventoryCommandNoRet addItemsCmd = new AddItemToInventoryCommandNoRet();
                    addItemsCmd.Items = items;
                    trader.GameObject.SendCommand(addItemsCmd);
                }
            }
        }
    }
}
