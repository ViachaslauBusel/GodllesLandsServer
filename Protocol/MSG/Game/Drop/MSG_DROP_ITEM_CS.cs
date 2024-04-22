using RUCP;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.MSG.Game.Drop
{
    /// <summary>
    /// Throw item from inventory to the ground
    /// </summary>
    [MessagePack(Opcode.MSG_DROP_ITEM, Channel.Reliable)]
    public struct MSG_DROP_ITEM_CS
    {
        public long ItemUID { get; set; }
        public int Count { get; set; }
        public Vector3 Position { get; set; }
    }
}
