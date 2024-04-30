using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DB
{
    public enum DatabaseSavePriority
    {
        /// <summary>
        /// Save evey tick
        /// </summary>
        SuperHigh = 4,
        /// <summary>
        /// Save every 1 min(600 ticks) 
        /// </summary>
        Hight = 3,
        /// <summary>
        /// Save every 10 min(6_000 ticks)
        /// </summary>
        Medium = 2,

        /// <summary>
        /// Save only when gameobject is destroyed
        /// </summary>
        Low = 1

    }
}
