using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UnitVisualization
{
    public abstract class BaseViewComponent : Component, IViewComponent
    {
        protected byte _version = 1;

    }
}
