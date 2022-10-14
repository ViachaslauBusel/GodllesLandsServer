
using BulletXNA.LinearMath;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Physics
{
   public static class Vector3RW
    {
        public static void WriteVector3(this Packet packet, Vector3 vector)
        {
            packet.WriteFloat((float)vector.X);
            packet.WriteFloat((float)vector.Y);
            packet.WriteFloat((float)vector.Z);
        }
        public static Vector3 ReadVector3(this Packet packet)
        {
            return new Vector3 (
            packet.ReadFloat(),
            packet.ReadFloat(),
            packet.ReadFloat()
            );
        }
    }
}
