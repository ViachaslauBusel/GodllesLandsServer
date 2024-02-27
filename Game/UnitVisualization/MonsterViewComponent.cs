using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UnitVisualization
{
    public class MonsterViewComponent : BaseViewComponent, IReadData<MonsterSkinData>
    {
        private int _skinID;

        public MonsterViewComponent(int skinID)
        {
            _skinID = skinID;
        }

        public override IViewComponent Clone()
        {
            return new MonsterViewComponent(_skinID);
        }

        public void UpdateData(ref MonsterSkinData data)
        {
            data.SkinID = _skinID;
            data.InNeedChaceVisual = _isNeedChaceVisual;
            data.VisualChaneObjectId = _visualChaneObjectId;
            data.Version = _version;
        }
    }
}
