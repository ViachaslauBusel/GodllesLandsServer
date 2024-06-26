﻿using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction.Commands
{
    /// <summary>
    /// This command send from interaction object to the interaction component on the character
    /// And finishes interaction with the object
    /// </summary>
    public struct InteractionEndNotificationCommand : ICommand
    {
        public int ObjectId { get; set; }
    }
}
