using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Map;
using Godless_Lands_Game.Physics;
using Godless_Lands_Game.Profiles;
using RUCP.Handler;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characters
{
    class CharacterListener
    {
        [Handler(Types.MapEntrance)]
        public static void LoadCharacter(Profile profile, Packet packet)
        {
            profile.Character.Load();

            World.Enter(profile.Character);

            packet = new Packet(packet.Client, RUCP.Channel.Reliable);
            packet.WriteType(Types.MapEntrance);
            packet.WriteVector3(profile.Character.Transform.position);
            packet.Send();
        }
    }
}
