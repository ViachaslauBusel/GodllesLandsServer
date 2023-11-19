using Game.Physics.PlayerInput.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics.PlayerInput.Scripts
{
    internal class PlayerInputComponent : Component, IReactCommand<PlayerInputCommand>
    {
        private TransformComponent m_transform;

        public override void Start()
        {
            m_transform = GetComponent<TransformComponent>();
        }
        public void ReactCommand(PlayerInputCommand command)
        {
            m_transform.UpdatePosition(command.Position, command.Rotation, command.Velocity, command.InMove);
        }
    }
}
