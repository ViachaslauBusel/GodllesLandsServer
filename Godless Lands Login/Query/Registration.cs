using Godless_Lands_Login.DBQuery;
using Protocol;
using Protocol.MSG.Login;
using RUCP;
using RUCP.Handler;

namespace Godless_Lands_Login.Query
{
    class Registration
    {
        [Handler(Opcode.MSG_REGISTRATION_Request)]
        public static void register(Profile profile, Packet packet)
        {

         packet.Read(out MSG_REGISTRATION_Request request);

         MSG_REGISTRATION_Response response = new MSG_REGISTRATION_Response();

            //The client version does not match the server
            if (request.Version != Login.version)
            {
                response.Notification = Protocol.Data.LoginInformationCode.WrongVersion;
                profile.Owner.Send(response);
                return;
            }



            //Login must not contain less than 3 or more than 30 characters
            if (request.Login.Length < 3 || request.Login.Length > 30)
            {
                response.Notification = Protocol.Data.LoginInformationCode.WrongLogin;
                profile.Owner.Send(response);
                return;
            }


            //Request whether the given login is in the database, if there is an exit with sending an error code to the client
            if (LoginTable.LoginExist(request.Login))//This login is already in use
            {
                response.Notification = Protocol.Data.LoginInformationCode.LoginExist;
                profile.Owner.Send(response);
                return;
            }


            LoginTable.CreateColumn(request.Login, request.Password);

            //Registration Successful
            response.Notification = Protocol.Data.LoginInformationCode.RegistrationSuccessful;
            profile.Owner.Send(response);
        }
    }
}
