using Database;
using Protocol;
using Protocol.MSG.Login;
using RUCP;
using RUCP.Handler;

namespace Godless_Lands_Login.Query
{
    class Registration
    {
        [Handler(Opcode.MSG_REGISTRATION_Request)]
        public static async void Register(Profile profile, Packet packet)
        {
            packet.Read(out MSG_REGISTRATION_CS request);

            MSG_REGISTRATION_SC response = new MSG_REGISTRATION_SC();

            //The client version does not match the server
            if (request.Version != Login.version)
            {
                response.Notification = Protocol.Data.LoginInformationCode.WrongVersion;
                profile.Client.Send(response);
                return;
            }

            //Login must not contain less than 3 or more than 30 characters
            if (request.Login.Length < 3 || request.Login.Length > 30)
            {
                response.Notification = Protocol.Data.LoginInformationCode.WrongLogin;
                profile.Client.Send(response);
                return;
            }

            //Request whether the given login is in the database, if there is an exit with sending an error code to the client
            if (await LoginDatabase.Provider.Call($"Call create_account('{request.Login}', '{request.Password}')"))
            {
                //Registration Successful
                response.Notification = Protocol.Data.LoginInformationCode.RegistrationSuccessful;
            }
            else //This login is already in use
            {
                response.Notification = Protocol.Data.LoginInformationCode.LoginExist;
            }

            profile.Client.Send(response);
        }
    }
}
