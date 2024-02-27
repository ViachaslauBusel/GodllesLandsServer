using NetworkGameEngine;
using Protocol.Data.Replicated.Drop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Drop
{
    public class DropHolderComponent : Component, IReadData<LootableUnitData>
    {

        public void UpdateData(ref LootableUnitData data)
        {
            data.Version = 1;
        }
    }
}
