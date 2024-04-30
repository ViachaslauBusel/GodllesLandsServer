using Database;
using Game.DB;
using Game.NetworkTransmission;
using NetworkGameEngine.JobsSystem;
using NetworkGameEngine.Units.Characters;
using Protocol.Data.Stats;
using Protocol.MSG.Game.Hotbar;
using Protocol.MSG.Game.ToClient.Stats;

namespace Game.Systems.Stats.Components
{
    internal class CharacterStatsComponent : StatsComponent, IDatabaseReadable
    {
        private CharacterInfoHolder m_characterInfoHolder;
        private NetworkTransmissionComponent m_networkTransmission;
        private List<StatField> m_dataForSync = new List<StatField>();
        private DBControlComponent m_dbControl;

        public override void Init()
        {
            base.Init();
            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_dbControl = GetComponent<DBControlComponent>();
            m_dbControl.OnDatabaseLoadComplete += OnDatabaseLoadComplete;
        }

        private void OnDatabaseLoadComplete()
        {

            MSG_LOAD_STATES msg = new MSG_LOAD_STATES
            {
                CharacterName = m_characterInfoHolder.CharacterName,
                Stats = m_dataForSync
            };

            foreach (var s in m_stats.Values)
            {
                msg.Stats.Add(s.Data);
                s.MarkDataAsSynced();
            }

            m_networkTransmission.Socket.Send(msg);
        }

        public async Job ReadFromDatabaseAsync()
        {
            m_characterInfoHolder = GetComponent<CharacterInfoHolder>();
            CharacterStat stat = await JobsManager.Execute(GameDatabase.Provider.Select<CharacterStat>($"SELECT get_chatacer_stat('{m_characterInfoHolder.CharacterID}')"));
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
