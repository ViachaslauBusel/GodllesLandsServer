using Protocol.Data.Replicated;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game
{
    [MessagePack(Opcode.MSG_GAMEOBJECT_UPDATE, RUCP.Channel.Reliable)]
    public struct MSG_GAMEOBJECT_UPDATE_SC
    {
        public int GameobjectID { get; set; }
        public List<IReplicationData> UpdatedComponents { get; set; }
        public List<int> RemovedComponents { get; set; }
    }
}
