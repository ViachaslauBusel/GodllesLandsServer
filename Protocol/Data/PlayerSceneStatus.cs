using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data
{
    public enum PlayerSceneStatus : byte
    {
        None,
        Loading,
        ReadyForSync,
    }
}
