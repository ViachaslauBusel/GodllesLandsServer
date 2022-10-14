using Godless_Lands_Game.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.DatabaseQuery
{
    class LoginTable
    {
        internal static int GetSessionKey(int id)
        {
            int SessionKey = 0;

            LDB.ExecuteQuery("SELECT sessionkey FROM login_table WHERE idlogin=" + id + ";", (dataReader) =>
            {
                if (dataReader.Read())
                {
                    SessionKey = dataReader.GetInt32(0);
                }
            });

            return SessionKey;
        }
    }
}
