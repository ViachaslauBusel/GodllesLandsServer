using Protocol.Data;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.MSG.Game.ToServer
{
    [MessagePack(Opcode.MSG_SCENE_STATUS, Channel.Queue)]
    public struct MSG_SCENE_STATUS
    {
        public PlayerSceneStatus Status { get; set; }
    }
}
