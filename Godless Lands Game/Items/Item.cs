using Game.DataSync;
using NetworkGameEngine.Debugger;
using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items
{
    public class Item : ClientAndDbSyncElement
    {
        private long _uniqueID;
        private readonly ItemInfo _data;
        private int _ownerID;
        private int _count;
        
        public long UniqueID => _uniqueID;
        public int OwnerID => _ownerID;
        public int Count => _count;
        public ItemInfo Data => _data;

        public Item(long uniqueID, ItemInfo data, int count)
        {
            _uniqueID = uniqueID;
            _data = data;
            _count = count;
        }

        internal void AddCount(int count)
        {
            _count += count;
            SetDataSyncPending();
        }

        internal void RemoveCount(int count)
        {
            if (_count < count)
            {
                Debug.Log.Fatal($"Cannot remove {count} items from item:{_uniqueID} because it has only {_count} items");
                return;
            }
            _count -= count;
            SetDataSyncPending();
        }

        internal void SetOwner(int ownerId)
        {
            _ownerID = ownerId;
            SetDataSyncPending();
        }

        internal void SetUniqueID(long uniqueID)
        {
            if (_uniqueID != 0)
            {
                Debug.Log.Fatal($"Cannot set unique id:{uniqueID} to item:{_uniqueID} because it is already set");
                return;
            }
            _uniqueID = uniqueID;
            SetDataSyncPending();
        }
    }
}
