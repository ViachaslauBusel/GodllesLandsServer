using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Animation
{
    public enum AnimationLayer : byte
    {
        Unknown = 0,
        TimeAnimation = 1,
        InstantAnimation = 2,
        StateAnimation = 3
    }
}
