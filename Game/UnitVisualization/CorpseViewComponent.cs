using Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.UnitVisualization
{
    public class CorpseViewComponent : BaseViewComponent, IReadData<CorpseSkinData>
    {
        private int _skinID;
        private int _cachedObjectId;

        public CorpseViewComponent(int skinID, int cachedObjectId)
        {
            _skinID = skinID;
            _cachedObjectId = cachedObjectId;
        }
      
        public void UpdateData(ref CorpseSkinData data)
        {
            data.SkinID = _skinID;
            data.CachedObjectId = _cachedObjectId;
            data.Version = _version;
        }
    }
}
