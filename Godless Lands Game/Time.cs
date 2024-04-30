using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Time
    {
        private static int _tick = 0;
        internal static long fixedDeltaTimeMillis = 100;
        internal static float fixedDeltaTimeSeconds = 0.1f;

        public static int Tick => _tick;

        public static void NextTick()
        {
            _tick++;
        }

        public static long Milliseconds => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
