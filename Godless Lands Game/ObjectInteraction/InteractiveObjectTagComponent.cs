using NetworkGameEngine;
using Protocol.Data.Replicated.ObjectInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction
{
    /// <summary>
    /// This component is responsible for marking the object as interactive.
    /// </summary>
    public class InteractiveObjectTagComponent : Component, IReadData<InteractionObjectData>
    {
        //private byte _version;
        //private bool _isInteractive;

        public void UpdateData(ref InteractionObjectData data)
        {
            data.Version = 1;
        }
    }
}
