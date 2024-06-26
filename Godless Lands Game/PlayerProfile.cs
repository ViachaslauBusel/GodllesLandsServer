﻿using Game.Main;
using Godless_Lands_Game.WorldEntry;
using NetworkGameEngine.Authorization;
using NetworkGameEngine.Debugger;
using RUCP;
using RUCP.Handler;

namespace NetworkGameEngine
{
    public interface IPlayerNetworkProfile
    {
        Client Client { get; }
        HandlersStorage<Action<PlayerProfile, Packet>> HandlersStorage { get; }
        int CharacterObjectID { get; }
    }
    public class PlayerProfile : BaseProfile, IPlayerNetworkProfile
    {
        public HandlersStorage<Action<PlayerProfile, Packet>> HandlersStorage { get; } = new HandlersStorage<Action<PlayerProfile, Packet>>();
        public AuthorizationHolder AuthorizationHolder { get; } = new AuthorizationHolder();
        public int CharacterObjectID { get; internal set; }
        public GameObject CharacterObject { get; internal set; }
        public int SelectedChacterID { get; internal set; }

        public PlayerProfile()
        {
            HandlersStorage.RegisterAllStaticHandlers();
        }

        public override void ChannelRead(Packet packet)
        {
            HandlersStorage.GetHandler(packet.OpCode)?.Invoke(this, packet);
        }

        public override void CheckingConnection()
        {

        }

        public override void CloseConnection(DisconnectReason reason)
        {
            Debug.Log.Debug($"connection closed: {reason}");
            PlayerWorldEntryController.Instance.Disconnect(AuthorizationHolder.LoginID);
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
