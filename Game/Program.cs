using Autofac;
using Autofac.Core;
using Cmd.Terminal;
using Database;
using Game.Items;
using Game.Main;
using Game.Physics.Terrain;
using Game.RespawnPoints;
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
            ItemsDataManager.Load();
            RespawnPointsStore.Load();

            GameDatabaseConfig.DatabaseName = "GD_GAME";
            LoginDatabaseConfig.DatabaseName = "GD_LOGIN";

            Server server = new Server(4343);
            server.SetHandler(() => new PlayerProfile());
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
