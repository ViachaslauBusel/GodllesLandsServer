using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Time
    {
        internal static float deltaTime = 0.1f;

        public static long Milliseconds => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
