using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UnitVisualization
{
    public class CharacterViewComponent : BaseViewComponent, IReadData<CharacterSkinData>
    {
        public override IViewComponent Clone()
        {
           return new CharacterViewComponent();
        }

        public void UpdateData(ref CharacterSkinData data)
        {
            data.InNeedChaceVisual = _isNeedChaceVisual;
            data.Version = _version;
        }
    }
}
