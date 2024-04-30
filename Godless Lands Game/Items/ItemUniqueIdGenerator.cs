using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items
{
    public class ItemUniqueIdGenerator
    {
        private long _currentID = -1;

        public ItemUniqueIdGenerator()
        {
            Initialize();
        }

        private async Task Initialize()
        {
            _currentID = await GameDatabase.Provider.Select<long>("SELECT get_unique_item_id()");
        }

        public long GetNextUniqueId()
        {
            if (_currentID != -1)
            {
                return _currentID++;
            }

            return -1;
        }
    }
}
