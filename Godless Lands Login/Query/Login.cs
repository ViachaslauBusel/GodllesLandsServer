using Godless_Lands_Login.DBQuery;
using Protocol;
using Protocol.MSG.Login;
using RUCP;
using RUCP.Handler;
using System;

namespace Godless_Lands_Login.Query
{
    class Login
    {
        public const short version = 5;
        private static Random random = new Random();

        [Handler(Opcode.MSG_AUTHORIZATION_Request)]
        public static void LogIn(Profile profile, Packet packet)
        {
            packet.Read(out MSG_AUTHORIZATION_Request request);

            MSG_AUTHORIZATION_Response response = new MSG_AUTHORIZATION_Response();

            //The client version does not match the server
            if (request.Version != version)
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


            //Query if the given login exists in the database
            string db_pass = LoginTable.GetPassword(request.Login);

            if (db_pass != null && db_pass.Equals(request.Password))
            {
                int sessionkey;
                lock (random)
                {
                    sessionkey = random.Next(Int32.MinValue, Int32.MaxValue);
                }
                LoginTable.SetSessionKey(request.Login, sessionkey);
                response.LoginID = LoginTable.GetID(request.Login);
                response.SessionKey = sessionkey;
                response.Notification = Protocol.Data.LoginInformationCode.AuthorizationSuccessful;
                profile.Owner.Send(response);
            }
            else  //Login or password is incorrect
            {
                response.Notification = Protocol.Data.LoginInformationCode.AuthorizationFail;
                profile.Owner.Send(response);
            }

        }
    }
}
