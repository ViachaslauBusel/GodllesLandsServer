﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated
{
    [MessageObject]
    public interface IReplicationData
    {
        public byte Version { get; }
    }
}
