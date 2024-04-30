using Database;
using Newtonsoft.Json;
using Protocol;
using Protocol.MSG.Login;
using RUCP;
using RUCP.Handler;
using System;
using System.Text.Json.Serialization;

namespace Godless_Lands_Login.Query
{
    class Login
    {
        class LoginData
        {
            [JsonPropertyName("login_id")]
            public int login_id;
            [JsonPropertyName("login_password")]
            public string login_password;
        }


        public const short version = 5;
        private static Random random = new Random();

        [Handler(Opcode.MSG_AUTHORIZATION_Request)]
        public static async void LogIn(Profile profile, Packet packet)
        {
            packet.Read(out MSG_AUTHORIZATION_CS request);

            MSG_AUTHORIZATION_SC response = new MSG_AUTHORIZATION_SC();

            //The client version does not match the server
            if (request.Version != version)
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

            LoginData loginData = new LoginData();
            string json = await LoginDatabase.Provider.SelectJson($"SELECT get_account('{request.Login}')");
            if (!string.IsNullOrEmpty(json))
            {
                JsonConvert.PopulateObject(json, loginData);
            }
           
            //Query if the given login exists in the database

            if (!string.IsNullOrEmpty(request.Password) && request.Password.Equals(loginData.login_password))
            {
                int sessionkey;
                lock (random)
                {
                    sessionkey = random.Next(Int32.MinValue, Int32.MaxValue);
                }
                if (await LoginDatabase.Provider.Call($"CALL set_session_token('{loginData.login_id}', '{sessionkey}')"))
                {
                    response.LoginID = loginData.login_id;
                    response.SessionKey = sessionkey;
                    response.Notification = Protocol.Data.LoginInformationCode.AuthorizationSuccessful;
                    profile.Client.Send(response);
                    return;
                }
            }
            //Login or password is incorrect
            {
                response.Notification = Protocol.Data.LoginInformationCode.AuthorizationFail;
                profile.Client.Send(response);
            }

        }
    }
}
