using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using Protocol.MSG.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UnitVisualization
{
    public class CharacterViewComponent : BaseViewComponent, IReadData<CharacterSkinData>
    {
        private int _weaponId;
        private int _toolId;

        public void UpdatePart(EquipmentType equipType, int partId)
        {
            switch (equipType)
            {
                case EquipmentType.WeaponRightHand:
                    _weaponId = partId;
                    break;
                case EquipmentType.PickaxeTool:
                    _toolId = partId;
                    break;
            }
            _version++;
        }

        public void UpdateData(ref CharacterSkinData data)
        {
            data.Version = _version;

            data.WeaponId = _weaponId;
            data.ToolId = _toolId;
        }
    }
}
