using Game.Loop;
using NetworkGameEngine.Authorization;
using NetworkGameEngine.Debugger;
using RUCP;
using RUCP.Handler;

namespace NetworkGameEngine
{
    public class Profile : BaseProfile
    {
        public HandlersStorage<Action<Profile, Packet>> handlersStorage = new HandlersStorage<Action<Profile, Packet>>();
        public AuthorizationHolder AuthorizationHolder { get; } = new AuthorizationHolder();
        public int CharacterObjectID { get; internal set; }
        public GameObject CharacterObject { get; internal set; }
        public int SelectedChacterID { get; internal set; }

        public Profile()
        {
            handlersStorage.RegisterAllStaticHandlers();
        }

        public override void ChannelRead(Packet packet)
        {
            handlersStorage.GetHandler(packet.OpCode)?.Invoke(this, packet);
        }


        public override void CheckingConnection()
        {

        }



        public override void CloseConnection(DisconnectReason reason)
        {
            Debug.Log.Debug($"connection closed: {reason}");
            Debug.Log.Debug($"removing character with id {CharacterObjectID}");
            if (CharacterObjectID != 0) { GameLoop.MainWorld.RemoveGameObject(CharacterObjectID); }
        }

        public override bool HandleException(Exception exception)
        {
            return false;
        }


        public override void OpenConnection()
        {
            //LoginID = packet.ReadInt();
            //SessionKey = packet.ReadInt();


            //int db_key = LoginTable.GetSessionKey(LoginID);
            //if (SessionKey != db_key)
            //{
            //    Console.WriteLine("SSK not valid: BD: " + db_key + " Client: " + SessionKey);
            //    return false;
            //}
            ///* character.login_id = id;
            // character.sessionkey = key;
            // character.socket = packet.getClient();

            // bool answer = World.addPlayer(character);*/

            //return true;
        }
    }
}
