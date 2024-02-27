using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items
{
    public class Item
    {
        private int _uniqueID;
        private ItemData _data;
        private int _count;
        
        public int UniqueID => _uniqueID;
        public ItemData Data => _data;
        public int Count => _count;

        public Item(int uniqueID, ItemData data, int count)
        {
            _uniqueID = uniqueID;
            _data = data;
            _count = count;
        }
    }
}
