using NetworkGameEngine;
using Protocol.Data.Replicated.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics.PlayerInput.Commands
{
    public struct PlayerInputCommand : ICommand
    {
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public bool InMove { get; set; }
        public float Velocity { get; set; }
        public MoveFlag MoveFlag { get; set; }
    }
}
