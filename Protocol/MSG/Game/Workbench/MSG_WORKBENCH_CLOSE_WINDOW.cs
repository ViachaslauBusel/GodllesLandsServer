using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Workbench
{
    [MessagePack(Opcode.MSG_WORKBENCH_CLOSE_WINDOW, Channel.Reliable)]
    public struct MSG_WORKBENCH_CLOSE_WINDOW
    {
    }
}
