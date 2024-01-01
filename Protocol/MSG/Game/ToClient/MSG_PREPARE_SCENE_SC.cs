using RUCP;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.MSG.Game.ToClient
{
    [MessagePack(Opcode.MSG_PREPARE_SCENE, Channel.Discard)]
    public struct MSG_PREPARE_SCENE_SC
    {
        public Vector3 EntryPoint { get; set; }
        public int GameObjectCharacterID { get; set; }
    }
}
