using Game.NetworkTransmission;
using Protocol.Data.Stats;
using Protocol.MSG.Game.ToClient.Stats;

namespace Game.Systems.Stats
{
    internal class CharacterStatsComponent : StatsComponent
    {
        private NetworkTransmissionComponent m_networkTransmission;
        private List<StatField> m_dataForSync = new List<StatField>();

        public override void Start()
        {
            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();

            MSG_LOAD_STATES msg = new MSG_LOAD_STATES
            {
                CharacterName = "test",
                Stats = m_dataForSync
            };

            foreach (var stat in m_stats.Values)
            {
                msg.Stats.Add(stat.Data);
                stat.MarkDataAsSynced();
            }

            m_networkTransmission.Socket.Send(msg);
        }

        public override void LateUpdate()
        {
            if (m_dataForSync.Count > 0) m_dataForSync.Clear();

            foreach (var stat in m_stats.Values)
            {
                if (stat.IsDirty)
                {
                    m_dataForSync.Add(stat.Data);
                    stat.MarkDataAsSynced();
                }
            }

            if (m_dataForSync.Count > 0)
            {
                var msg = new MSG_UPDATE_STATES
                {
                    Stats = m_dataForSync
                };

                m_networkTransmission.Socket.Send(msg);
            }
        }
    }
}
