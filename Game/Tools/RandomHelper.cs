using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Tools
{
    public static class RandomHelper
    {
        private static Random _random;

        static RandomHelper()
        {
            _random = new Random();
        }

        public static int Range(int min, int max)
        {
            return _random.Next(min, max);
        }
        public static float Range(float min, float max)
        {
            return (float)_random.NextDouble() * (max - min) + min;
        }
    }
}
