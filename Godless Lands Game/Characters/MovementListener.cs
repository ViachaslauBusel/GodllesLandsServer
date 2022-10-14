using BulletXNA.LinearMath;
using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Map;
using Godless_Lands_Game.Physics;
using Godless_Lands_Game.Profiles;
using RUCP.Handler;
using RUCP;
using RUCP.Packets;
using RUCP.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using RUCP.Client;

namespace Godless_Lands_Game.Characters
{
   public class MovementListener
    {
        [Handler(-1)]
        public static void Move(Profile profile, Packet packet)
        {
            CharacterTransform transform = profile.Character.Transform;
            lock (transform)
            {
                //Отбрасывание устаревших пакетов >>
                short syncNumber = packet.ReadShort();
                if (NumberUtils.ShortCompare(syncNumber, transform.syncNumber) <= 0) return;
                transform.syncNumber = syncNumber;
                //<<<

                Vector3 newPosition = packet.ReadVector3();
                transform.rotation = packet.ReadFloat();
                bool endMove = packet.ReadBool();


                
                //  double speed = 

                  transform.position = SpeedControl.Check(profile.Character, newPosition);


                //Если игрок перешел в другую локацию
                Location location = Location.IfChanged(transform);
                if (!location.IsEmpty)
                {
                    Console.WriteLine($"Персонаж {profile.Character.Stats.Name} сменил локацию {profile.Character.Transform.location} -> {location}");
                    //  Message.systemMessage(character,"сменил локацию: "+character.getTransform().location+" на локацию: "+_loc);
                    Tile.ChangeLocation(profile.Character, location);
                }

                SendAllPosition(profile.Character, endMove);
            }
        }

       
        [Handler(Types.Rotation)]
        public static void Rotation(Profile profile, Packet packet)
        {

            Transform transform = profile.Character.Transform;
            lock (transform)
            {
                transform.rotation = packet.ReadFloat();

              SendAllRotation(profile.Character);
            }
        }

        private static void SendAllRotation(Character character)
        {
            foreach (Tile tile in new TileAround(character.Transform.location))
            {
                foreach (Character otherClient in tile.Players)
                {

                    if (character.Equals(otherClient)) continue;

                    Packet packet = new Packet(otherClient.Socket, Channel.Discard);
                    packet.WriteType(Types.CharacterRotation);

                    packet.WriteInt(character.ID);
                    packet.WriteFloat(character.Transform.rotation);

                    packet.Send();
                }
            }
        }

        public static void SendAllPosition(Character character, bool endMove)
        {

            foreach (Tile tile in new TileAround(character.Transform.location))
            {
                foreach (Character otherClient in tile.Players)
                {

                    if (character.Equals(otherClient)) continue;

                    Packet packet = new Packet(otherClient.Socket, endMove? Channel.Reliable : Channel.Unreliable);
                    packet.WriteType(Types.CharacterMove);

                    packet.WriteInt(character.ID);
                    packet.WriteShort(character.Transform.syncNumber);
                    packet.WriteVector3(character.Transform.position);
                    packet.WriteFloat(character.Transform.rotation);
                    packet.WriteBool(endMove);
                    packet.Send();
                }
            }
        }
    }
}
