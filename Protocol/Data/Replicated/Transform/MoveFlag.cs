using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Transform
{
    public enum MoveFlag : byte
    {
        None = 0,
        Jump = 1,
    }
}
