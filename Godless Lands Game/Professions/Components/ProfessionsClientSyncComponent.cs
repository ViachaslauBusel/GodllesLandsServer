﻿using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol.MSG.Game.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Professions.Components
{
    internal class ProfessionsClientSyncComponent : Component
    {
        private ProfessionsComponent _professions;
        private NetworkTransmissionComponent _networkTransmissionComponent;
        private List<ProfessionSyncData> _syncData = new List<ProfessionSyncData>();

        public override void Init()
        {
            _professions = GetComponent<ProfessionsComponent>();
            _networkTransmissionComponent = GetComponent<NetworkTransmissionComponent>();
        }

        public override void LateUpdate()
        {
            // Check if any profession has data that needs to be synced with the client
            if (_professions.Professions.Any(p => p.IsDataSyncWithClientPending))
            {
                foreach (Profession profession in _professions.Professions)
                {
                    // If data is pending, add it to the sync data list and mark it as synced with the client
                    if (profession.IsDataSyncWithClientPending)
                    {
                        _syncData.Add(new ProfessionSyncData(profession.ProfessionType, profession.Level, profession.Experience, profession.ExpForLevelUp));
                        profession.MarkDataAsSyncedWithClient();
                    }
                }
                // Send the sync data to the client
                MSG_PROFESSIONS_SYNC_SC msg = new MSG_PROFESSIONS_SYNC_SC();
                msg.Professions = _syncData;
                _networkTransmissionComponent.Socket.Send(msg);

                _syncData.Clear();
            }
        }
    }
}
