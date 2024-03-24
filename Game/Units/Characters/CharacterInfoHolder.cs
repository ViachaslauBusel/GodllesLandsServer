using Database;
using Game.DB;
using Game.Systems.Stats;
using NetworkGameEngine.JobsSystem;
using RUCP;

namespace NetworkGameEngine.Units.Characters
{
    public class CharacterInfoHolder : Component, IDatabaseReadable
    {
        public int CharacterID { get; init; }
        public string CharacterName { get; private set; }

        public CharacterInfoHolder(int characterID)
        {
            CharacterID = characterID;
        }

        public async Job ReadFromDatabaseAsync()
        {
            CharacterName = await JobsManager.Execute(GameDatabaseProvider.SelectObject<string>($"SELECT get_character_nickname('{CharacterID}')"));
        }
    }
}
