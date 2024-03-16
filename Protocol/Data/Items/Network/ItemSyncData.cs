using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Items.Network
{
    [MessageObject]
    public struct ItemSyncData
    {   
        public long UniqueID { get; set; }
        public int ItemID { get; set; }
        public int Count { get; set; }
        public int SlotIndex { get; set; }
    }
}
