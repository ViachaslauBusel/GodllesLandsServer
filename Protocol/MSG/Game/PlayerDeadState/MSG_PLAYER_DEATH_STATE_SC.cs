using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.PlayerDeadState
{
    [MessagePack(Opcode.MSG_PLAYER_DEATH_STATE, Channel.Queue)]
    public struct MSG_PLAYER_DEATH_STATE_SC
    {
        public bool IsAlive { get; set; }
    }
}
