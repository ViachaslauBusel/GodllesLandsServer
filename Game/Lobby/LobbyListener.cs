using Database;
using Game.GameObjectFactory;
using Game.Main;
using Game.NetworkTransmission;
using Game.Physics;
using Game.Physics.DynamicObjects;
using Game.Physics.PlayerInput.Scripts;
using Game.UnitVisualization;
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
        public static async void Entrance(PlayerProfile profile, Packet packet)
        {
           // packet.Read(out MSG_LOBBY_ENTRANCE_CS request);

            string charactersJsonData = await GameDatabaseProvider.SelectJson($"SELECT get_characters('{profile.AuthorizationHolder.LoginID}')");

            CharacterJsonData[] characters = string.IsNullOrEmpty(charactersJsonData) ? new CharacterJsonData[0] : JsonConvert.DeserializeObject<List<CharacterJsonData>>(charactersJsonData).ToArray();//string.IsNullOrEmpty(charactersJsonData) ? new CharacterData[0] :

            MSG_CHARACTERS_LIST_SC response = new MSG_CHARACTERS_LIST_SC();
            response.CharacterDatas = characters.Select(c => new CharacterData() { CharacterID = c.CharacterID, CharacterName = c.CharacterName }).ToArray();
            profile.Client.Send(response); 
        }

        [Handler(Opcode.MSG_SELECT_CHARACTER)]
        public static async void SelectCharacter(PlayerProfile profile, Packet packet)
        {
            packet.Read(out MSG_SELECT_CHARACTER_CS request);
            MSG_SELECT_CHARACTER_SC response = new MSG_SELECT_CHARACTER_SC();

            if (await GameDatabaseProvider.SelectObject($"SELECT is_character_belong('{profile.AuthorizationHolder.LoginID}', '{request.CharacterID}')") is bool isBelong && isBelong && profile.CharacterObjectID == 0)
            {//Если персонаж принадлежит игроку
                response.InformationCode = LoginInformationCode.AuthorizationSuccessful;

               //GameObject character = CharacterFactory.Create(request.CharacterID, profile.Owner, profile.handlersStorage);

               //response.CharacterObjectID = profile.CharacterObjectID = await GameLoop.MainWorld.AddGameObject(character);
               profile.SelectedChacterID = request.CharacterID;
            }
            else//error
            {
                response.InformationCode = LoginInformationCode.AuthorizationFail;
            }
            profile.Client.Send(response);
        }
    }
}
