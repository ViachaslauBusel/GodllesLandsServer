using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data
{
    public enum LoginInformationCode : byte
    {
        //The client version does not match the server
        WrongVersion = 1,
        //Login must not contain less than 3 or more than 30 characters
        WrongLogin = 2,
        //Login or password is incorrect
        AuthorizationFail = 3,
        AuthorizationSuccessful = 4,
        //This login is already in use
        LoginExist = 5,
        RegistrationSuccessful = 6,
        ConnectionFail = 7,
        WrongPassword = 8,
    }
}
