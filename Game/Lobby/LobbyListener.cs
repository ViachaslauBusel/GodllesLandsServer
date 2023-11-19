﻿using Database;
using Game.Loop;
using Game.Physics;
using Game.Physics.DynamicObjects;
using Game.Physics.PlayerInput.Scripts;
using Game.Skins;
using NetworkGameEngine.Units.Characters;
using Newtonsoft.Json;
using Protocol;
using Protocol.Data;
using Protocol.MSG.Game;
using RUCP;
using RUCP.Handler;

namespace NetworkGameEngine.Lobby
{
    struct CharacterJsonData
    {
        [JsonProperty("character_id")]
        public int CharacterID { get; set; }

        [JsonProperty("character_name")]
        public string CharacterName { get; set; }
    }
    class LobbyListener
    {

        [Handler(Opcode.MSG_LOBBY_ENTRANCE)]
        public static async void Entrance(Profile profile, Packet packet)
        {
           // packet.Read(out MSG_LOBBY_ENTRANCE_CS request);

            string charactersJsonData = await GameDatabaseProvider.SelectJson($"SELECT get_characters('{profile.AuthorizationHolder.LoginID}')");

            CharacterJsonData[] characters = string.IsNullOrEmpty(charactersJsonData) ? new CharacterJsonData[0] : JsonConvert.DeserializeObject<List<CharacterJsonData>>(charactersJsonData).ToArray();//string.IsNullOrEmpty(charactersJsonData) ? new CharacterData[0] :

            MSG_CHARACTERS_LIST_SC response = new MSG_CHARACTERS_LIST_SC();
            response.CharacterDatas = characters.Select(c => new CharacterData() { CharacterID = c.CharacterID, CharacterName = c.CharacterName }).ToArray();
            profile.Owner.Send(response); 
        }

        [Handler(Opcode.MSG_SELECT_CHARACTER)]
        public static async void SelectAsync(Profile profile, Packet packet)
        {
            packet.Read(out MSG_SELECT_CHARACTER_CS request);
            MSG_SELECT_CHARACTER_SC response = new MSG_SELECT_CHARACTER_SC();

            if (await GameDatabaseProvider.SelectObject($"SELECT is_character_belong('{profile.AuthorizationHolder.LoginID}', '{request.CharacterID}')") is bool isBelong && isBelong && profile.CharacterObjectID == 0)
            {//Если персонаж принадлежит игроку
                response.InformationCode = LoginInformationCode.AuthorizationSuccessful;

                GameObject character = new GameObject();
                character.AddComponent(new CharacterInfoHolder(request.CharacterID, profile.Owner));
                character.AddComponent(new TransformComponent());
                character.AddComponent(new CharacterViewComponent());
                character.AddComponent(new DynamicObjectComponent());
                character.AddComponent(new PlayerInputComponent());

               response.CharacterObjectID = profile.CharacterObjectID = await GameLoop.MainWorld.AddGameObject(character);
               profile.CharacterObject = character;
            }
            else//error
            {
                response.InformationCode = LoginInformationCode.AuthorizationFail;
            }
            profile.Owner.Send(response);
        }
    }
}