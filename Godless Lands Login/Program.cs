using Cmd.Terminal;
using Godless_Lands_Login.Database;
using Godless_Lands_Login.Servers;
using RUCP;
using System;

namespace Godless_Lands_Login
{
    class Program
    {
        static void Main(string[] args)
        {
             DatabaseHandler.Initialize();
             ServerReader.Load();
             Server server = new Server(3737);
             server.SetHandler(() => new Profile()); 
             server.Start();

            Terminal.Listen();
        }
    }
}
