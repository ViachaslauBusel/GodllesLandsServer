using Protocol.Data.Items.Network;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Drop
{
    [MessagePack(Opcode.MSG_DROP_LIST_SYNC, Channel.Reliable)]
    public struct MSG_DROP_LIST_SYNC_SC
    {
        public bool OpenWindow { get; set; }
        public List<ItemSyncData> SyncData { get; set; }

    }
}
