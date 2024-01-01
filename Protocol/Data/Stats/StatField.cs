using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Stats
{
    [MessageObject]
    public struct StatField
    {
        public StatCode Code { get; set; }
        public int Value { get; set; }
    }
}
