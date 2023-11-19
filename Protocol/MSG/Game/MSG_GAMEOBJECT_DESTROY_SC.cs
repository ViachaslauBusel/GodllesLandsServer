using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_GAMEOBJECT_DESTROY, Channel.Reliable)]
    public struct MSG_GAMEOBJECT_DESTROY_SC
    {
        public List<int> DestroyedObjects { get; set; }
    }
}
