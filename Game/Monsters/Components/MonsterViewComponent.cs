using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Monsters.Components
{
    public class MonsterViewComponent : Component, IReadData<MonsterSkinData>
    {
        private int _skinID;

        public MonsterViewComponent(int skinID)
        {
            _skinID = skinID;
        }

        public void UpdateData(ref MonsterSkinData data)
        {
            data.SkinID = _skinID;
            data.Version = 1;
        }
    }
}
