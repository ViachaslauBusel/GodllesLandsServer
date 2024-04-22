using Protocol.Data.Items.Network;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Trade
{
    [MessagePack(Opcode.MSG_SYNC_TRADE_WINDOW, Channel.Reliable)]
    public struct MSG_SYNC_TRADE_WINDOW_SC
    {
        public int TradeID { get; set; }
        public bool IsOwner { get; set; }
        public int BagSize { get; set; }
        public List<ItemSyncData> Items { get; set; }
    }
}
