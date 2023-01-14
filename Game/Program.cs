using Cmd.Terminal;
using Database;
using RUCP;
using System;

namespace NetworkGameEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            GameDatabaseConfig.DatabaseName = "GL_Game";
            LoginDatabaseConfig.DatabaseName = "GL_Login";

            Server server = new Server(4343);
            server.SetHandler(() => new Profile());
            server.Start(new ServerOptions()
            {
                MaxParallelism = 4,
                Mode = ServerMode.Automatic
            });


            Terminal.Listen();
        }
    }
}
