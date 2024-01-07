using DataFileProtocol.Skills;
using Game.NetworkTransmission;
using Game.Systems.TargetSystem;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.Skills;
using RUCP;

namespace Game.Skills
{
    public class SkillUsageComponent : Component
    {
        private SkillsStoreComponent _skillsStore;
        private NetworkTransmissionComponent _networkTransmission;
        private UnitTargetSelectionComponent _unitTargetSelection;

        public override void Start()
        {
            _unitTargetSelection = GetComponent<UnitTargetSelectionComponent>();
            _skillsStore = GetComponent<SkillsStoreComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _networkTransmission.RegisterHandler(Opcode.MSG_SKILL_USE, SkillUse);
        }

        private void SkillUse(Packet packet)
        {
            packet.Read(out MSG_SKILL_USE_CS msg);

            Skill skill = _skillsStore.GetSkill(msg.SkillID);

            if (skill == null)
                return;
   

            skill.Use(_unitTargetSelection.Target);
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_SKILL_USE);
        }
    }
}
