using Game.Physics.PlayerInput.Commands;
using NetworkGameEngine;
using Protocol.Data.Replicated.Transform;

namespace Game.Physics.PlayerInput.Scripts
{
    internal class PlayerInputComponent : Component, IReactCommand<PlayerInputCommand>
    {
        private TransformComponent m_transform;

        public override void Start()
        {
            m_transform = GetComponent<TransformComponent>();
        }
        public void ReactCommand(ref PlayerInputCommand command)
        {
            byte version = m_transform.UpdatePosition(command.Position, command.Rotation, command.Velocity, command.InMove);

            if(command.MoveFlag.HasFlag(MoveFlag.Jump))
            {
                m_transform.PushEvent(new TransformEvent()
                {
                    MoveFlag = MoveFlag.Jump,
                    Position = command.Position,
                    EventHappenedAtVersion = version
                });
            }
        }
    }
}
