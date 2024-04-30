using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.UnitVisualization
{
    internal interface ICachedViewComponent
    {
        int CachedObjectId { get; }
        int SkinId { get; }

        void SetInNeedChaceVisual(bool value);
    }
}
