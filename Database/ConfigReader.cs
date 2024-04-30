using System.Xml;

namespace Database
{
    internal class ConfigReader
    {
        public string ServerAddress { get; private set; }

        public ConfigReader(string configName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(configName);

            string database = doc.DocumentElement["database"].InnerText;
            string address = doc.DocumentElement["address"].InnerText;
            string port = doc.DocumentElement["port"].InnerText;
            string username = doc.DocumentElement["username"].InnerText;
            string password = doc.DocumentElement["password"].InnerText;

            ServerAddress = $"Server={address}; Username={username}; Database={database}; Port={port}; Password={password}; SSLMode=Prefer; MaxPoolSize=100;";
        }
    }
}
