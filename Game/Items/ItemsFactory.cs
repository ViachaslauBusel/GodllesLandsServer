using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items
{
    internal class ItemsFactory
    {
        public Item CreateItem(int id, int count)
        {
            ItemData data = ItemsDataManager.GetData(id);
            if (data == null) return null;
                
            return new Item(0, data, count);
        }
    }
}
