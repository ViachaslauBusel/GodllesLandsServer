using Game.Systems.Stats;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items.ItemUsageLogic
{
    internal class RestorePointUsageLogic : IItemUsageLogic
    {
        private BodyComponent _bodyComponent;

        public void Init(Component component)
        {
            _bodyComponent = component.GetComponent<BodyComponent>();
        }

        public bool Use(Item item)
        {
            RestorePointsItemData data = (RestorePointsItemData)item.Data;

            if (data == null)
            {
                Debug.Log.Error($"Item {item.UniqueID} has wrong data type");
                return false;
            }

            _bodyComponent.Heal(data.RestoreHP);
            return true;
        }
    }
}
