using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Physics
{
   public static class TransformRW
    {
        public static void WriteTransform(this Packet packet, Transform transform)
        {
            packet.WriteShort(transform.syncNumber);
            packet.WriteVector3(transform.position);
            packet.WriteFloat(transform.rotation);
        }

    }
}
