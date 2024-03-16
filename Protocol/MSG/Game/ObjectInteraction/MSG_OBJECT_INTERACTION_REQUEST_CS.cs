using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.ObjectInteraction
{
    public enum InteractionState : byte
    {
        StartInteraction,
        EndInteraction
    }

    [MessagePack(Opcode.MSG_OBJECT_INTERACTION_REQUEST, Channel.Reliable)]
    public struct MSG_OBJECT_INTERACTION_REQUEST_CS
    {
        public InteractionState State { get; set; }
        public int ObjectId {  get; set; }
    }
}
