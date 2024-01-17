﻿namespace Database
{
    public static class GameDatabaseConfig
    {
        private static string m_server_address = $"Server={m_serverIP}; Username={m_username}; Database={m_databaseName}; Port={m_serverPort}; Password={m_password}; SSLMode=Prefer; MaxPoolSize=100;";
        private static string m_databaseName = "null";
        private static string m_serverIP = "16.171.232.248";//"***REMOVED***";
        private static int m_serverPort = 5432;
        private static string m_username = "postgres";
        private static string m_password = "5475269qaZ";//"***REMOVED***";
        public static string DatabaseName
        {
            set { m_databaseName = value; UpdateAddress(); }
        }
        public static string ServerIP
        {
            set { m_serverIP = value; UpdateAddress(); }
        }
        public static int ServerPort
        {
            set { m_serverPort = value; UpdateAddress(); }
        }
        public static string Username
        {
            set { m_username = value; UpdateAddress(); }
        }
        public static string Password
        {
            set { m_password = value; UpdateAddress(); }
        }


        internal static string SERVER_ADDRESS => m_server_address;

        private static void UpdateAddress()
        {
            m_server_address = $"Server={m_serverIP}; Username={m_username}; Database={m_databaseName}; Port={m_serverPort}; Password={m_password}; SSLMode=Prefer; MaxPoolSize=100;";
        }
    }
}