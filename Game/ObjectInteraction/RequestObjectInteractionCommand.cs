using NetworkGameEngine;
using Protocol.MSG.Game.ObjectInteraction;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction
{
    public struct RequestObjectInteractionCommand : ICommand
    {
        public IPlayerNetworkProfile PlayerProfile { get; set; }
        public InteractionState State { get; internal set; }
    }
}
