using NetworkGameEngine;
using Protocol.MSG.Game.CombatMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.CombatModeControl.Components
{
    public class CombatModeComponent : Component, IReadData<CombotModeData>
    {
        private byte _version = 0;
        private bool _isInCombatMode = false;

        public bool CombatMode => _isInCombatMode;

        public void SwitchToCombatMode()
        {
            _isInCombatMode = true;
            _version++;
        }

        internal void SwitchToPeaceMode()
        {
            _isInCombatMode = false;
            _version++;
        }

        public void UpdateData(ref CombotModeData data)
        {
            data.CombatMode = _isInCombatMode;
            data.Version = _version;
        }
    }
}
