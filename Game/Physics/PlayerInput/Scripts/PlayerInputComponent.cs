using Game.NetworkTransmission;
using Game.Physics.Transform;
using NetworkGameEngine;
using Protocol;
using Protocol.Data.Replicated.Transform;
using Protocol.MSG.Game;
using RUCP;

namespace Game.Physics.PlayerInput.Scripts
{
    internal class PlayerInputComponent : Component
    {
        private TransformComponent m_transform;
        private NetworkTransmissionComponent m_networkTransmission;

        public override void Start()
        {
            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_transform = GetComponent<TransformComponent>();

            m_networkTransmission.RegisterHandler(Opcode.MSG_PLAYER_INPUT_CS, PlayerInputProcces);
        }

        private void PlayerInputProcces(Packet packet)
        {
            packet.Read(out MSG_PLAYER_INPUT_CS input);
            byte version = m_transform.UpdatePosition(input.Position, input.Rotation, input.Velocity, input.InMove);


            if (input.MoveFlag.HasFlag(MoveFlag.Jump))
            {
                m_transform.PushEvent(new TransformEvent()
                {
                    MoveFlag = MoveFlag.Jump,
                    Position = input.Position,
                    EventHappenedAtVersion = version
                });
            }
        }
    }
}
