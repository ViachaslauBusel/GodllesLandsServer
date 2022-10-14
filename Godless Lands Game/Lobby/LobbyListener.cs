using Godless_Lands_Game.Characters;
using Godless_Lands_Game.DatabaseQuery;
using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Profiles;
using RUCP;
using RUCP.Handler;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Lobby
{
    class LobbyListener
    {
        [Handler(Types.LobbyEntrance)]
        public static void Entrance(Profile profile, Packet packet)
        {
            var characters = CharactersTable.GetCharacters(profile.LoginID);


            packet = new Packet(packet.Client, Channel.Reliable);
            packet.WriteType(Types.MyCharacters);
            foreach (var character in characters)
            {
                packet.WriteInt(character.chatacterID);
                packet.WriteString(character.characterName);
            }
            packet.Send();
        }

        [Handler(Types.SelectCharacter)]
        public static void Select(Profile profile, Packet pack)
        {
            int characterID = pack.ReadInt();

            Packet packet = new Packet(pack.Client, Channel.Reliable);
            packet.WriteType(Types.SelectCharacter);
            if (CharactersTable.IsBelong(profile.LoginID, characterID) && profile.Character == null)
            {//Если персонаж принадлежит игроку
                packet.WriteByte(0);//ok

                profile.Character = new Character(pack.Client, characterID);
            }
            else packet.WriteByte(1);//error

            packet.Send();
        }
    }
}
