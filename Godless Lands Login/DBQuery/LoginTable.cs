using Godless_Lands_Login.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Login.DBQuery
{
    class LoginTable
    {
        internal static string GetPassword(string login)
        {
            String password = null;

            DatabaseHandler.ExecuteQuery("SELECT password FROM login_table WHERE login='" + login + "';", (dataReader) =>
            {
                if (dataReader.Read())
                {
                    password = dataReader.GetString(0);
                }
            });
            
            return password;
        }

        internal static void SetSessionKey(string login, int sessionkey)
        {
            DatabaseHandler.ExecuteUpdate("UPDATE login_table SET sessionkey=" + sessionkey + " WHERE login='" + login + "';");
        }

        internal static bool LoginExist(string login)
        {
            bool answer = false;
            DatabaseHandler.ExecuteQuery("SELECT login FROM login_table WHERE login='" + login + "';", (dataReader) =>
            {
                answer = dataReader.Read();
            });
            return answer;
        }

        internal static void CreateColumn(string login, string password)
        {
            DatabaseHandler.ExecuteUpdate("INSERT INTO login_table (login,password) VALUES('" + login + "','" + password + "');");
        }

        internal static int GetID(string login)
        {
            int ID = -1;

            DatabaseHandler.ExecuteQuery("SELECT idlogin FROM login_table WHERE login='" + login + "';", (dataReader) =>
            {
                if (dataReader.Read())
                {
                    ID = dataReader.GetInt32(0);
                }
            });

            return ID;
        }
    }
}
