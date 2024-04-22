using Game.DataSync;
using Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Trade
{
    public class TradeCell : ClientAndDbSyncElement
    {
        public Item Item { get; private set; }
        public int SlotIndex { get; private set; }
        public bool IsEmpty => Item == null;

        public TradeCell(int slotIndex)
        {
            SlotIndex = slotIndex;
        }

        public void SetItem(Item item)
        {
            Item = item;
            SetDataSyncPendingOnlyWithClient();
        }

        public Item TakeItem()
        {
            Item item = Item;
            Item = null;
            SetDataSyncPendingOnlyWithClient();
            return item;
        }
    }
}
