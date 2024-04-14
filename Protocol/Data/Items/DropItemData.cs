using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Items
{
    public class DropItemData
    {
        public int ItemID { get; set; }
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
        public float Chance { get; set; }
    }
}
