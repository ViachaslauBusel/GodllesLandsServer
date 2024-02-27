using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Items
{
    public class ItemData
    {
        public readonly int ID;
        public readonly bool IsStackable;
        public readonly int Weight;

        public ItemData(int id, bool isStackable, int weight)
        {
            ID = id;
            IsStackable = isStackable;
            Weight = weight;
        }
    }
}
