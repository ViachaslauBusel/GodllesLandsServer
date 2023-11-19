using RUCP;

namespace NetworkGameEngine.Units.Characters
{
    public class CharacterInfoHolder : Component
    {
        public int CharacterID { get; init; }
        public Client Socket { get; init; }

        public CharacterInfoHolder(int characterID, Client client)
        {
            CharacterID = characterID;
            Socket = client;
        }
    }
}
