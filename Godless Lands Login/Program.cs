using Cmd.Terminal;
using Database;
using Godless_Lands_Login.Servers;
using RUCP;

namespace Godless_Lands_Login
{
    class Program
    {
        static void Main(string[] args)
        {
             LoginDatabaseConfig.DatabaseName = "GL_Login";

             ServerReader.Load();
             Server server = new Server(3737);
             server.SetHandler(() => new Profile()); 
             server.Start();

             Terminal.Listen();
        }
    }
}
