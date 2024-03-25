using RUCP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Protocol.MSG.Game.Drop
{
    public enum DropCommandType : byte
    {
        TakeItem,
        TakeAllItems,
        EndInteraction
    }
    [MessagePack(Opcode.MSG_TAKE_DROP, Channel.Reliable)]
    public struct MSG_DROP_COMMAND_CS
    {
        public DropCommandType CommandType { get; set; }
        /// <summary>
        /// If DropID = 0, that means take all items
        /// </summary>
        public int DropIndex { get; set; }
    }
}
