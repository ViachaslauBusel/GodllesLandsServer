using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.CombatMode;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.CombatModeControl.Components
{
    public class CombatModeListenerComponent : Component
    {
        private NetworkTransmissionComponent _networkTransmission;
        private CombatModeComponent _combatMode;

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _combatMode = GetComponent<CombatModeComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_SWITCH_COMBAT_MODE, SwitchCombatMode);
        }

        private void SwitchCombatMode(Packet packet)
        {
            packet.Read(out MSG_SWITCH_COMBAT_MODE_CS request);

            if (_combatMode.CombatMode)
            {
                _combatMode.SwitchToPeaceMode();
            }
            else
            {
                _combatMode.SwitchToCombatMode();
            }

        }
    }
}
