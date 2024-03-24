using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Tools
{
    public static class Vector3Extension
    {
        public static Vector3 Normalize(this Vector3 value, float length)
        {
            return value / length;
        }

        public static Vector3 ClearY(this Vector3 value)
        {
            return new Vector3(value.X, 0, value.Z);
        }
    }
}
