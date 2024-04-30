using Godless_Lands_Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UnitVisualization
{
    public class MonsterViewComponent : BaseViewComponent, ICachedViewComponent, IReadData<MonsterSkinData>
    {
        private int _skinID;
        private bool _inNeedChaceVisual = false;

        public int SkinId => _skinID;

        public int CachedObjectId => GameObject.ID;

        public void SetInNeedChaceVisual(bool value)
        {
            _inNeedChaceVisual = value;
            _version++;
        }

        public MonsterViewComponent(int skinID)
        {
            _skinID = skinID;
        }

        public void UpdateData(ref MonsterSkinData data)
        {
            data.SkinID = _skinID;
            data.Version = _version;
            data.InNeedChaceVisual = _inNeedChaceVisual;
        }
    }
}
