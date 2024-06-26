﻿using Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;

namespace Godless_Lands_Game.UnitVisualization
{
    public class NpcViewComponent : BaseViewComponent, IReadData<NpcSkinData>
    {
        private int _skinID;

        public NpcViewComponent(int skinID)
        {
            _skinID = skinID;
        }

        public void UpdateData(ref NpcSkinData data)
        {
            data.SkinID = _skinID;
            data.Version = _version;
        }
    }
}
