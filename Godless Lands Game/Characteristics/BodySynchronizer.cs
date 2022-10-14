using Godless_Lands_Game.Handler;
using RUCP;
using RUCP.Client;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characteristics
{
    class BodySynchronizer
    {
        public static void PlayerUpdate(Body body, ClientSocket socket)
        {
            Packet packet = new Packet(socket, Channel.Discard);
            packet.WriteType(Types.HPViewUpdate);
            packet.WriteInt(body.HP);
            packet.WriteInt(body.MaxHP);
            packet.WriteInt(body.MP);
            packet.WriteInt(body.MaxMP);
            packet.WriteInt(body.Stamina);
            packet.WriteInt(body.MaxStamina);
            packet.Send();
        }
    }
}
