using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Skins
{
    public class CharacterViewComponent : Component, IReadData<CharacterSkinData>
    {
        
        public void UpdateData(ref CharacterSkinData data)
        {
            
        }
    }
}
