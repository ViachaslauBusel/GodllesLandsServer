using Protocol.Data.Stats;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.ToClient.Stats
{
    [MessagePack(Opcode.MSG_LOAD_STATES, Channel.Queue)]
    public struct MSG_LOAD_STATES
    {
        public string CharacterName { get; set; }
        public List<StatField> Stats { get; set; }
    }
}
