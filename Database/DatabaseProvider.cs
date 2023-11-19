﻿using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class DatabaseProvider
    {
        public async static Task<bool> Call(string cmd, string serverAddress)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(serverAddress))
                {
                    await connection.OpenAsync();
                    using (NpgsqlCommand command =
                    new NpgsqlCommand(cmd, connection))
                    {
                         command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch(Exception e) { Console.WriteLine(e);  }
            return false;
        }

        public async static Task<string> SelectJson(string cmd, string serverAddress)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(serverAddress))
                {
                    await connection.OpenAsync();
                    using (NpgsqlCommand command = new NpgsqlCommand(cmd, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader[0].ToString();
                            }
                        }
                    }
                }
            }catch { }
            return null;
        }
        public static async Task<T> Select<T>(string cmd, string serverAddress)
        {
            string objectJson = await SelectJson(cmd, serverAddress);
            return JsonConvert.DeserializeObject<T>(objectJson);
        }
        public async static Task<object> SelectObject(string cmd, string serverAddress)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(serverAddress))
                {
                    await connection.OpenAsync();
                    using (NpgsqlCommand command = new NpgsqlCommand(cmd, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader[0];
                            }
                        }
                    }
                }
            }
            catch { }
            return null;
        }
    }
}