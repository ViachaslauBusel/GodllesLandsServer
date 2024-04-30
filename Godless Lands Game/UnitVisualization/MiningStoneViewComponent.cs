using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UnitVisualization
{
    public class MiningStoneViewComponent : BaseViewComponent, IReadData<MiningStoneSkinData>
    {
        private int _skinID;

        public MiningStoneViewComponent(int skinID)
        {
            _skinID = skinID;
        }

        public void UpdateData(ref MiningStoneSkinData data)
        {
            data.SkinID = _skinID;
            data.Version = _version;
        }
    }
}
