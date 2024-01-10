using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Tools
{
    internal class FloatHelper
    {
        public static string FloatToString(float value)
        {
            return value.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
