﻿using Database;
using Game.Items;
using Game.Main;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.Units.Characters;
using Newtonsoft.Json;
using Protocol;
using Protocol.MSG.Game;
using RUCP;
using RUCP.Handler;
using System.Text.Json.Serialization;

namespace NetworkGameEngine.Lobby
{
    static class CharacterCreator
    {
        struct CharacterData
        {
            [Newtonsoft.Json.JsonProperty("login_id")]
            public int LoginID;
            [Newtonsoft.Json.JsonProperty("character_id")]
            public int CharacterID;
        }

        [Handler(Opcode.MSG_CREATE_CHARACTER)]
        public static async void CreateAsync(PlayerProfile profile, Packet packet)
        {
            if (!profile.AuthorizationHolder.IsLoggedIn) return;

            packet.Read(out MSG_CREATE_CHARACTER_CS request);
            MSG_CREATE_CHARACTER_SC response = new MSG_CREATE_CHARACTER_SC();

            if (request.Name.Length < 3 || request.Name.Length > 30)
            {
                response.InformationCode = Protocol.Data.LoginInformationCode.WrongLogin;
                profile.Client.Send(response);
                return;
            }

            bool isCreatedCharacter = await GameDatabase.Provider.Call($"CALL create_character('{profile.AuthorizationHolder.LoginID}', '{request.Name}')");
            if (!isCreatedCharacter)
            {
                response.InformationCode = Protocol.Data.LoginInformationCode.WrongLogin;
                profile.Client.Send(response);
                return;
            }
            string characterData_json = await GameDatabase.Provider.SelectJson($"SELECT get_chatacer_data('{request.Name}')");
         
            bool isCharacterExist = !string.IsNullOrEmpty(characterData_json);
            if (!isCharacterExist)
            {
                response.InformationCode = Protocol.Data.LoginInformationCode.WrongLogin;
                profile.Client.Send(response);
                return;
            }

            CharacterData characterData = JsonConvert.DeserializeObject<CharacterData>(characterData_json);


            if (characterData.LoginID != profile.AuthorizationHolder.LoginID)
            {
                response.InformationCode = Protocol.Data.LoginInformationCode.WrongLogin;
                profile.Client.Send(response);
                return;
            }

            bool isCreatedPosition = await GameDatabase.Provider.Call($"CALL set_character_position('{characterData.CharacterID}', {1180.0f}, {183.0f}, {1884.0f})");
            if (!isCreatedPosition) { Debug.Log.Fatal($"Failed to set position when creating character"); }

            await GameDatabase.Provider.SelectObject<bool>($"CALL insert_into_items_given_offline('{characterData.CharacterID}', {4}, {1})");


            response.InformationCode = Protocol.Data.LoginInformationCode.AuthorizationSuccessful;
            profile.Client.Send(response);

            ////Одеть оружеи на персонажа
            //int characterID = CharactersTable.GetCharacterID(name);
            //if (characterID != 0)
            //{
            //    /*  Item weapon = ItemList.create(4);
            //      weapon.setLocation(ItemLocation.Armor);
            //      weapon.setOwner_id(characterID);
            //      weapon.create();*/
            //}
        }
    }
}
