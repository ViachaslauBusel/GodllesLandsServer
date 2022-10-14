using Protocol.MSG.Login;
using RUCP;

namespace Godless_Lands_Login.Servers
{
    class ServerList
    {
        public static void GetList(Client client, Packet pack)
        {
            MSG_SERVER_LIST server_list = new MSG_SERVER_LIST();
            server_list.Servers = ServerReader.Servers;
            client.Send(server_list);
        }
    }
}
