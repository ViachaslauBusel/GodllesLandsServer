using Godless_Lands_Game.Characters;
using Godless_Lands_Game.Map;
using RUCP;
using RUCP.Handler;
using System;

namespace Godless_Lands_Game.Profiles
{
    public class Profile : BaseProfile
    {
        public static HandlersStorage<Action<Profile, Packet>> handlersStorage = new HandlersStorage<Action<Profile, Packet>>();
        public int LoginID { get; private set; }
        public int SessionKey { get; private set; }
        public Character Character { get; set; }

        public override void ChannelRead(Packet packet)
        {
            handlersStorage.GetHandler(packet.OpCode)(this, packet);
        }


        public override void CheckingConnection()
        {
        
        }

 

        public override void CloseConnection(DisconnectReason reason)
        {
            World.Exit(Character);
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
