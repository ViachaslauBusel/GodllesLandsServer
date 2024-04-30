using Game.NetworkTransmission;
using Protocol.MSG.Game.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Skills
{
    public class CharacterSkillsStoreComponent : SkillsStoreComponent
    {
        private NetworkTransmissionComponent _networkTransmission;
        private List<int> _syncData = new List<int>();

        public override void Start()
        {
            _syncData.Clear();
            _syncData.AddRange(_skills.Keys);

            _networkTransmission = GetComponent<NetworkTransmissionComponent>();

            MSG_SKILLS_UPDATE msg = new MSG_SKILLS_UPDATE();
            msg.Skills = _syncData;

            _networkTransmission.Socket.Send(msg);
        }
    }
}
