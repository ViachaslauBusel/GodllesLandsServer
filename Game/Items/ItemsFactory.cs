using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.Items
{
    public class ItemsFactory
    {
        private readonly ItemUniqueIdGenerator _uniqueIdGenerator;


        public ItemsFactory(ItemUniqueIdGenerator uniqueIdGenerator)
        {
            _uniqueIdGenerator = uniqueIdGenerator;
        }

        public Item CreateItem(int id, long uniqueId, int count)
        {
            ItemData data = ItemsDataManager.GetData(id);
            if (data == null) return null;

            return new Item(uniqueId, data, count);
        }
    }
}
