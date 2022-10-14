
using BulletXNA.LinearMath;
using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Physics;
using Godless_Lands_Game.Profiles;
using RUCP;
using RUCP.Handler;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Terrain
{
    class TerrainTest
    {
        [Handler(Types.TestRay)]
        public static void Test(Profile profile, Packet packet)
        {
          Packet  packetAnswer = new Packet(profile.Character.Socket, Channel.Reliable);
            packetAnswer.WriteType(Types.TestRay);
            int ray = 0;
            long startMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            while (packet.AvailableBytes > 3 * sizeof(float))
            {

                Vector3 v1 = packet.ReadVector3();
                //  Vector3f v2 = new Vector3f(v1);
                //    v1.x -= ((int) ( v1.x / 100.0f)) * 100.0f;
                // v1.z -= ((int) ( v1.z / 100.0f)) * 100.0f;
                //    System.out.println(v1);
                //  v2.x = v2.x % 100.0f;
                //  v2.z = v2.z % 100.0f;

                Vector3 hit = Ray.rayDown(v1);
                ray++;
               

                if (hit != Vector3.Zero)
                {
                    packetAnswer.WriteBool(true);
                    packetAnswer.WriteVector3(hit);
                }
                else
                {
                    packetAnswer.WriteBool(false);
                }
            }
            Console.WriteLine($"ray: {ray} millis: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startMillis}");
            packetAnswer.Send();
        }
    }
}
