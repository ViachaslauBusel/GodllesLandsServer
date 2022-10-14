using RUCP;
using RUCP.Handler;
using System;

namespace Godless_Lands_Login
{
    class Profile : BaseProfile
    {
        public static HandlersStorage<Action<Profile, Packet>> handlersStorage = new HandlersStorage<Action<Profile, Packet>>();


        public Profile()
        {
     //      handlersStorage.RegisterResolver<MSG_AUTHORIZATION_Request>(Opcode.MSG_AUTHORIZATION);
      //     handlersStorage.RegisterAllResolvers();
           handlersStorage.RegisterAllStaticHandlers();
        }
        public override void ChannelRead(Packet packet)
        {
            handlersStorage.GetHandler(packet.OpCode)(this, packet);
        }

        public override void CheckingConnection()
        {
           
        }

        public override void CloseConnection(DisconnectReason reason)
        {
           
        }

        public override bool HandleException(Exception exception)
        {
          return false;
        }

        public override void OpenConnection()
        {
         
        }
    }
}
