using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.Data.Workbenches
{
    public class WorkbenchData
    {
        public WorkbenchType WorkbenchType { get; set; }
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
    }
}
