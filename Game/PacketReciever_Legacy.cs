using RUCP.Handler;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using NetworkGameEngine;
using Protocol.MSG.Game;
using Game.Physics.PlayerInput.Commands;
using NetworkGameEngine.Debugger;

namespace Game
{
    internal static class PacketReciever_Legacy
    {
        [Handler(Opcode.MSG_PLAYER_INPUT_CS)]
        public static void PlayerInput (Profile profile, Packet packet)
        {
            packet.Read(out MSG_PLAYER_INPUT_CS playerInput);


            profile.CharacterObject.SendCommand(new PlayerInputCommand()
            {
                Position = playerInput.Position,
                Rotation = playerInput.Rotation,
                Velocity = playerInput.Velocity,
                InMove = playerInput.InMove,
                MoveFlag = playerInput.MoveFlag
            });
        }
    }
}
