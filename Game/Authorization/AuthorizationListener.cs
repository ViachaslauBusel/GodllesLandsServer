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

namespace NetworkGameEngine.Authorization
{
    public static class AuthorizationListener
    {
        [Handler(Opcode.MSG_GAME_AUTHORIZATION)]
        public static async void Entrance(PlayerProfile profile, Packet packet)
        {
            packet.Read(out MSG_GAME_AUTHORIZATION_CS request);
            MSG_GAME_AUTHORIZATION_SC response = new MSG_GAME_AUTHORIZATION_SC();

            object sessionToken_object = await LoginDatabaseProvider.SelectObject($"SELECT get_session_token('{request.LoginID}')");

            if (sessionToken_object == null)
            {
                response.InformationCode = Protocol.Data.LoginInformationCode.WrongLogin;
                profile.Client.Send(response);
                return;
            }
            int sessionToken = (int)sessionToken_object;
            if (sessionToken != request.SessionToken)
            {
                response.InformationCode = Protocol.Data.LoginInformationCode.WrongPassword;
                profile.Client.Send(response);
                return;
            }
            profile.AuthorizationHolder.Login(request.LoginID, request.SessionToken);

            response.InformationCode = Protocol.Data.LoginInformationCode.AuthorizationSuccessful;
            profile.Client.Send(response);
        }
    }
}
