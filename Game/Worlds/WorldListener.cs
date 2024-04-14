using Game.GameObjectFactory;
using Game.Main;
using Godless_Lands_Game.WorldEntry;
using Protocol;
using RUCP;
using RUCP.Handler;

namespace NetworkGameEngine.Worlds
{
    static class WorldListener
    {
        [Handler(Opcode.MSG_WORLD_ENTRANCE)]
        public static async void Entrance(PlayerProfile profile, Packet packet)
        {
            //if (!GameLoop.MainWorld.TryGetGameObject(profile.CharacterObjectID, out GameObject character))
            //{
            //    Debug.Log.Fatal($"unable to find character with specified id");
            //    return;
            //}
            //while (!character.IsInitialized) { await Task.Delay(1); }


            profile.CharacterObject = CharacterFactory.Create(profile.SelectedChacterID, profile);

            bool result = PlayerWorldEntryController.Instance.ConnectCharacterObject(profile.AuthorizationHolder.LoginID, profile.CharacterObject);

            if (result)
            {
                profile.CharacterObjectID = await GameLoop.MainWorld.AddGameObject(profile.CharacterObject);
            }
        }
    }
}
