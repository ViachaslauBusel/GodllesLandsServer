using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Map;
using Godless_Lands_Game.Physics;
using Godless_Lands_Game.Profiles;
using RUCP;
using RUCP.Handler;

namespace Godless_Lands_Game.Characters
{
    class CharacterListener
    {
        [Handler(Types.MapEntrance)]
        public static void LoadCharacter(Profile profile, Packet packet)
        {
            profile.Character.Load();

            World.Enter(profile.Character);

            packet = Packet.Create(RUCP.Channel.Reliable);
            packet.OpCode = (Types.MapEntrance);
            packet.WriteVector3(profile.Character.Transform.position);
            profile.Owner.Send(packet);
        }
    }
}
