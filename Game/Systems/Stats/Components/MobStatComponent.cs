using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Stats.Components
{
    internal class MobStatComponent : StatsComponent
    {
        public MobStatComponent(string name)
        {
            m_name = name;
        }
    }
}
