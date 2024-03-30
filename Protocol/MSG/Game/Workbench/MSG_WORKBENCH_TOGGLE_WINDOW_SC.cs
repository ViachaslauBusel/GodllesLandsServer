using Protocol.Data.Workbenches;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Workbench
{
    [MessagePack(Opcode.MSG_WORKBENCH_TOGGLE_WINDOW, Channel.Queue)]
    public struct MSG_WORKBENCH_TOGGLE_WINDOW_SC
    {
        public bool IsOpen { get; set;}
        public bool IsReadyForWork { get; set; }
        public WorkbenchType WorkbenchType { get; set;}
       
    }
}
