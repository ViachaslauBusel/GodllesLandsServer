using Protocol;
using Protocol.MSG.Login;
using RUCP;
using RUCP.Handler;

namespace Godless_Lands_Login.Servers
{
    class ServerList
    {
        [Handler(Opcode.MSG_SERVER_LIST)]
        public static void GetList(Profile profile, Packet packet)
        {
            MSG_SERVER_LIST_SC server_list = new MSG_SERVER_LIST_SC();
            server_list.Servers = ServerReader.Servers;
            profile.Owner.Send(server_list);
        }
    }
}
