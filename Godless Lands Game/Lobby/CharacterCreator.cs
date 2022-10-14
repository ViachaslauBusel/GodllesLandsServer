
using BulletXNA.LinearMath;
using Godless_Lands_Game.DatabaseQuery;
using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Profiles;
using RUCP;
using RUCP.Client;
using RUCP.Handler;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Lobby
{
    class CharacterCreator
    {
        [Handler(Types.OwnCharacterCreate)]
        public static void Create(Profile profile, Packet packet)
        {

            string name = packet.ReadString();

            if (name.Length < 3 || name.Length > 30) { SendAnswer(packet.Client, 1); return; }

            if (CharactersTable.NameExist(name)) { SendAnswer(packet.Client, 2); return; } //Имя уже используется

            CharactersTable.CreateCharacter(name, profile.LoginID);
            CharacterPosition.SavePosition(CharactersTable.GetCharacterID(name), new Vector3(1180.0f, 183.0f, 1884.0f));

            SendAnswer(packet.Client, 10);

            //Одеть оружеи на персонажа
            int characterID = CharactersTable.GetCharacterID(name);
            if (characterID != 0)
            {
              /*  Item weapon = ItemList.create(4);
                weapon.setLocation(ItemLocation.Armor);
                weapon.setOwner_id(characterID);
                weapon.create();*/
            }
        }

        //Имя не должно содержать меньше 3 или больше 30 символов = 1;
        //Имя уже используется = 2
        //Создание прошло успешно = 10
        private static void SendAnswer(ClientSocket client, int code)
        {
            Packet packet = new Packet(client, Channel.Reliable);
            packet.WriteType(Types.OwnCharacterCreate);
            packet.WriteInt(code);
            packet.Send();
        }
    }
}
