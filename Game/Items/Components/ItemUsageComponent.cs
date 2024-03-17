using Game.Items.ItemUsageLogic;
using NetworkGameEngine;
using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items.Components
{
    internal class ItemUsageComponent : Component
    {
        private Dictionary<Type, IItemUsageLogic> _itemUsageLogics;

        public override void Init()
        {
            _itemUsageLogics = new Dictionary<Type, IItemUsageLogic>();
            _itemUsageLogics.Add(typeof(RestorePointsItemData), new RestorePointUsageLogic());

            foreach (var item in _itemUsageLogics.Values)
            {
                item.Init(this);
            }
        }

        public bool Use(Item item)
        {
            if (_itemUsageLogics.TryGetValue(item.Data.GetType(), out IItemUsageLogic logic))
            {
                return logic.Use(item);
            }

            return false;
        }

        internal bool IsUsableItem(Item item)
        {
            return _itemUsageLogics.ContainsKey(item.Data.GetType());
        }
    }
}
