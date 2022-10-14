using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Database
{
    class GDB
    {
        public static MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder();

        public static void Initialize()
        {
            stringBuilder.Server = "127.0.0.1";
            stringBuilder.UserID = "yazZ3va";
            stringBuilder.Password = "5475269qaz";
            stringBuilder.Database = "gdb";
        }
        public static void ExecuteQuery(string query, Action<MySqlDataReader> action)
        {
            DatabaseHandler.ExecuteQuery(stringBuilder, query, action);
        }
        public static void ExecuteUpdate(string query)
        {
            DatabaseHandler.ExecuteUpdate(stringBuilder, query);
        }
    }
}
