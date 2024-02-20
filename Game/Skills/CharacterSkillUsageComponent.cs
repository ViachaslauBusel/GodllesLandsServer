using DataFileProtocol.Skills;
using Game.NetworkTransmission;
using Game.Systems.TargetSystem;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.Skills;
using RUCP;

namespace Game.Skills
{
    public class CharacterSkillUsageComponent : SkillUsageComponent
    {
        private NetworkTransmissionComponent _networkTransmission;

        public override void Start()
        {
            base.Start();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _networkTransmission.RegisterHandler(Opcode.MSG_SKILL_USE, SkillUse);
        }

        private void SkillUse(Packet packet)
        {
            packet.Read(out MSG_SKILL_USE_CS msg);
            UseSkill(msg.SkillID);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _networkTransmission.UnregisterHandler(Opcode.MSG_SKILL_USE);
        }
    }
}
