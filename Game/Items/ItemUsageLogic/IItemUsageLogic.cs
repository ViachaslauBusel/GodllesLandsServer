using Game.Items.Components;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items.ItemUsageLogic
{
    public interface IItemUsageLogic
    {
        void Init(Component component);
        bool Use(Item item);
    }
}
