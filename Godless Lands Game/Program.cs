using Godless_Lands_Game.Database;
using Godless_Lands_Game.Map;
using Godless_Lands_Game.Profiles;
using RUCP;
using System;

namespace Godless_Lands_Game
{
    class Program
    {
        static void Main(string[] args)
        {

            LDB.Initialize();
            GDB.Initialize();
            World.Load();
            Server server = new Server(3232);
            server.SetHandler(() => new Profile());
            server.Start(new ServerOptions()
            {
              MaxParallelism = 4,
              Mode = ServerMode.Manual
            });


            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "stop":
                        server.Stop();
                        return;
                    default:
                        Console.WriteLine("команда не распознана");
                        break;
                }

            }
        }
    }
}
