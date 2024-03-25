using NetworkGameEngine;
using Protocol.Data.Replicated.ObjectInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction.MiningStone
{
    public class MiningStoneInteractionComponent : Component, IReadData<InteractionObjectData>
    {
        public void UpdateData(ref InteractionObjectData data)
        {
            data.Version = 1;
        }
    }
}
