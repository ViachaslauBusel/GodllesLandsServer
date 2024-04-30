using RUCP.Handler;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using Protocol.MSG.Game;
using Database;
using Godless_Lands_Game.WorldEntry;
using Protocol.Data;

namespace NetworkGameEngine.Authorization
{
    public static class AuthorizationListener
    {
        [Handler(Opcode.MSG_GAME_AUTHORIZATION)]
        public static async void Entrance(PlayerProfile profile, Packet packet)
        {
            packet.Read(out MSG_GAME_AUTHORIZATION_CS request);
            MSG_GAME_AUTHORIZATION_SC response = new MSG_GAME_AUTHORIZATION_SC();

            object sessionToken_object = await LoginDatabase.Provider.SelectObject($"SELECT get_session_token('{request.LoginID}')");

            if (sessionToken_object == null)
            {
                response.InformationCode = LoginInformationCode.WrongLogin;
                profile.Client.Send(response);
                return;
            }
            int sessionToken = (int)sessionToken_object;
            if (sessionToken != request.SessionToken)
            {
                response.InformationCode = LoginInformationCode.WrongPassword;
                profile.Client.Send(response);
                return;
            }

            if(PlayerWorldEntryController.Activated == false)
            {
                response.InformationCode = LoginInformationCode.ServerNotReady;
                profile.Client.Send(response);
                return;
            }

            // Attempt to add the player to the list of players
            // If such a player is already in the list, it returns false
            if (PlayerWorldEntryController.Instance.Connect(request.LoginID, profile) == false)
            {
                // If the player is already in the game, disconnect them
                PlayerWorldEntryController.Instance.InitializeDisconnect(request.LoginID);
                response.InformationCode = LoginInformationCode.AlreadyInGame;
                profile.Client.Send(response);
                return;
            }

            profile.AuthorizationHolder.Login(request.LoginID, request.SessionToken);

            response.InformationCode = LoginInformationCode.AuthorizationSuccessful;
            profile.Client.Send(response);
        }
    }
}
