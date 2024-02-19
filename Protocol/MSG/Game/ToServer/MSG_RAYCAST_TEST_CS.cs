using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.MSG.Game.ToServer
{
    [MessagePack(Opcode.MSG_RAYCAST_TEST, RUCP.Channel.Reliable)]
    public struct MSG_RAYCAST_TEST_CS
    {
    }
}
