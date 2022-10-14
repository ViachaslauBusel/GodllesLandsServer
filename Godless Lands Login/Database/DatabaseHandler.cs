using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Login.Database
{
   public class DatabaseHandler
    {
       public static MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder();

        public static void Initialize()
        {
            stringBuilder.Server = "127.0.0.1";
            stringBuilder.UserID = "yazZ3va";
            stringBuilder.Password = "5475269qaz";
            stringBuilder.Database = "ldb";
        }
        
        public static void ExecuteQuery(string query, Action<MySqlDataReader> action)
        {
            using (MySqlConnection conn = new MySqlConnection(stringBuilder.ToString()))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = query;
                    using (MySqlDataReader dataReader = command.ExecuteReader())
                    {
                        action(dataReader);
                    }
                }
            }
        }

        public static void ExecuteUpdate(string query)
        {
            using (MySqlConnection conn = new MySqlConnection(stringBuilder.ToString()))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
