using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Database
{
    class DatabaseHandler
    {
        public static void ExecuteQuery(MySqlConnectionStringBuilder stringBuilder, string query, Action<MySqlDataReader> action)
        {
            try
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
            }catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void ExecuteUpdate(MySqlConnectionStringBuilder stringBuilder, string query)
        {
            try
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
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
