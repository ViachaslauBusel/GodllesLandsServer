using RUCP.Handler;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;

namespace NetworkGameEngine.Worlds
{
    static class WorldListener
    {
        [Handler(Opcode.MSG_WORLD_ENTRANCE)]
        public static async void Entrance(Profile profile, Packet packet)
        {
         
        }
    }
}
