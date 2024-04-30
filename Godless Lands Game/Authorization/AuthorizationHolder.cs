using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGameEngine.Authorization
{
    public class AuthorizationHolder
    {
        public int LoginID { get; private set; }
        public int SessionKey { get; private set; }
        public bool IsLoggedIn { get; private set; } = false;

        public void Login(int loginID, int sessionKey)
        {
            if (LoginID != 0)
            {
                Debug.Log.Fatal($"Reauthorization attempt. Login in:{LoginID}, out:{loginID}");
                return;
            }
            LoginID = loginID;
            SessionKey = sessionKey;
            IsLoggedIn = true;
        }
    }
}
