using Protocol.Data.Workbenches;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Replicated.Skins
{
    public struct WorkbenchSkinData : IReplicationData
    {
        public byte Version { get; set; }
        public WorkbenchType WorkbenchType { get; set; }
    }
}
