using NetworkGameEngine;
using Protocol.Data.Replicated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics.DynamicObjects
{
    public class DynamicObjectComponent : Component, IReadData<DynamicObjectData>
    {
        public void UpdateData(ref DynamicObjectData data)
        {

        }
    }
}
