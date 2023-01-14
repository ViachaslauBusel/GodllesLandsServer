using Godless_Lands_Game.Handler;
using RUCP;
using RUCP;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characteristics
{
    class BodySynchronizer
    {
        public static void PlayerUpdate(Body body, Client socket)
        {
            Packet packet = Packet.Create(Channel.Discard);
            packet.OpCode = (Types.HPViewUpdate);
            packet.WriteInt(body.HP);
            packet.WriteInt(body.MaxHP);
            packet.WriteInt(body.MP);
            packet.WriteInt(body.MaxMP);
            packet.WriteInt(body.Stamina);
            packet.WriteInt(body.MaxStamina);
            socket.Send(packet);
        }
    }
}
