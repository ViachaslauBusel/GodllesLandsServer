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
        protected bool _isNeedChaceVisual = false;
        protected byte _version = 1;
        protected int _visualChaneObjectId;

        public abstract IViewComponent Clone();

        public void SetNeedChaceVisual(bool isNeedChaceVisual)
        {
            Debug.Log.Info($"SetNeedChaceVisual {isNeedChaceVisual}");
            _isNeedChaceVisual = isNeedChaceVisual;
            _version++;
        }

        public void SetVisualObjecId(int id)
        {
            _visualChaneObjectId = id;
            _version++;
        }
    }
}
