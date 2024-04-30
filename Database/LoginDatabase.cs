using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class LoginDatabase
    {
        public static DatabaseProvider Provider { get; private set; }

        static LoginDatabase()
        {
            ConfigReader configReader = new ConfigReader("Configs/LoginDatabase.config");
            Console.WriteLine(configReader.ServerAddress);
            Provider = new DatabaseProvider(configReader.ServerAddress);
        }
    }
}
