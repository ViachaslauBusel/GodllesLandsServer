using Game.NetworkTransmission;
using Game.Physics.PlayerInput.Components;
using Game.Physics.Transform;
using NetworkGameEngine;
using Protocol;
using Protocol.Data.Replicated.Transform;
using Protocol.MSG.Game;
using RUCP;
using System.Numerics;

namespace Game.Physics.PlayerInput.Scripts
{
    internal class PlayerInputComponent : Component
    {
        private TransformComponent _transform;
        private MovementSpeedValidatorComponent _movementSpeedValidator;
        private NetworkTransmissionComponent _networkTransmission;

        public override void Start()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _transform = GetComponent<TransformComponent>();
            _movementSpeedValidator = GetComponent<MovementSpeedValidatorComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_PLAYER_INPUT_CS, PlayerInputProcces);
        }

        private void PlayerInputProcces(Packet packet)
        {
            packet.Read(out MSG_PLAYER_INPUT_CS input);

            Vector3 newPostion = _movementSpeedValidator.Check(input.Position, out bool isNeedCorrect);

            if (isNeedCorrect == false)
            {
                byte version = _transform.UpdatePosition(input.Position, input.Rotation, input.Velocity, input.InMove);


                if (input.MoveFlag.HasFlag(MoveFlag.Jump))
                {
                    _transform.PushEvent(new TransformEvent()
                    {
                        MoveFlag = MoveFlag.Jump,
                        Position = input.Position,
                        EventHappenedAtVersion = version
                    });
                }
            }
            else
            {
                _transform.TeleportTo(newPostion);
            }
           
        }
    }
}
