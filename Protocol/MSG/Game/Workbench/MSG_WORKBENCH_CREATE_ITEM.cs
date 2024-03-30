using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.Workbench
{
    [MessagePack(Opcode.MSG_WORKBENCH_CREATE_ITEM, Channel.Reliable)]
    public struct MSG_WORKBENCH_CREATE_ITEM
    {
        public int RecipeID { get; set; }
        public List<long> Components { get; set; }
        public List<long> Fuel { get; set; }
    }
}
