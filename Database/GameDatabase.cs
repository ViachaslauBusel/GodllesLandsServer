using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Database
{
    public static class GameDatabase
    {
        public static DatabaseProvider Provider { get; private set; }

        static GameDatabase()
        {
            ConfigReader configReader = new ConfigReader("Configs/GameDatabase.config");
            Provider = new DatabaseProvider(configReader.ServerAddress);
        }
    }
}
