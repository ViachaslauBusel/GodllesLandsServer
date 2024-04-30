using Newtonsoft.Json;
using Npgsql;

namespace Database
{
    public class DatabaseProvider
    {
        private string _serverAddress;

        public DatabaseProvider(string serverAddress)
        {
            _serverAddress = serverAddress;
        }

        public async Task<bool> Call(string cmd)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_serverAddress))
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

        public async Task<string> SelectJson(string cmd)
        {
                using (NpgsqlConnection connection = new NpgsqlConnection(_serverAddress))
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
            return null;
        }

        public async Task<T> Select<T>(string cmd)
        {
            string objectJson = await SelectJson(cmd);
            return JsonConvert.DeserializeObject<T>(objectJson);
        }

        public async Task<object> SelectObject(string cmd)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_serverAddress))
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
            return null;
        }

        public async Task<T> SelectObject<T>(string cmd)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_serverAddress))
            {
                await connection.OpenAsync();
                using (NpgsqlCommand command = new NpgsqlCommand(cmd, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (T)reader[0];
                        }
                    }
                }
            }
            return default;
        }
    }
}
