using Autofac;
using Autofac.Core;
using Cmd.Terminal;
using Database;
using Game.Loop;
using Game.Skills.Data;
using Game.Skills.Handler;
using NetworkGameEngine.Debugger;
using RUCP;
using System;

namespace NetworkGameEngine
{
    class Program
    {
        static void Main(string[] args)
        {
          //  var builder = new ContainerBuilder();
          //  builder.RegisterType<ConsoleOutput>().As<IOutput>();
          //  builder.RegisterType<TodayWriter>().As<IDateWriter>();
            //var Container = builder.Build();
            //Container.Resolve<ICommand>(new NamedParameter("characterID", 124));

            SkillsDataStore.Load();

            GameDatabaseConfig.DatabaseName = "GL_Game";
            LoginDatabaseConfig.DatabaseName = "GL_Login";

            Server server = new Server(4343);
            server.SetHandler(() => new Profile());
            server.Start(new ServerOptions()
            {
                MaxParallelism = 4,
                Mode = ServerMode.Automatic
            });

            GameLoop.Start(server);

            Debug.Log.Info("Server started");
            Terminal.Listen();
        }
    }
}
