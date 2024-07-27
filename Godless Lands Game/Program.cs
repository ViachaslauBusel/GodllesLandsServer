using Cmd.Terminal;
using Database;
using Game.Items;
using Game.Main;
using Game.RespawnPoints;
using Game.Skills.Data;
using Godless_Lands_Game.Quests.Data;
using NetworkGameEngine.Debugger;
using RUCP;

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
            ItemsDataRegistry.Load();
            RespawnPointsStore.Load();
            QuestsDataStore.Load();

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
