using RUCP;

namespace NetworkGameEngine.Units.Characters
{
    public class CharacterInfoHolder : Component
    {
        public int CharacterID { get; init; }
       

        public CharacterInfoHolder(int characterID)
        {
            CharacterID = characterID;
        }
    }
}
