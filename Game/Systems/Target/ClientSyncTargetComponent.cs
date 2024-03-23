using Game.NetworkTransmission;
using Game.Systems.Stats;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Replicated;
using Protocol.MSG.Game.ToClient.Target;

namespace Game.Systems.Target
{
    /// <summary>
    /// This component is synced data of target with client
    /// </summary>
    public class ClientSyncTargetComponent : Component
    {
        private HealtData m_lastSyncHealtData;
        private TargetManagerComponent m_targetManager;
        private NetworkTransmissionComponent m_networkTransmission;
        private GameObject m_target;


        public override void Init()
        {
            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_targetManager = GetComponent<TargetManagerComponent>();
            m_targetManager.OnTargetChanged += TargetChanged;
        }

        private void TargetChanged(GameObject @object)
        {
            m_target = @object;
            FullUpdateTarget();
        }

        protected void HPUpdateTarget()
        {
            if (m_target == null)
            {
                Debug.Log.Error("UnitTargetSelectionComponent: Target is null");
                return;
            }

            m_target.ReadData(out HealtData healtData);
            MSG_UNIT_TARGET_HP_SC response = new MSG_UNIT_TARGET_HP_SC();
            response.PercentHP = (int)((healtData.HP / (float)healtData.MaxHP) * 100);
            m_networkTransmission.Socket.Send(response);
            m_lastSyncHealtData = healtData;
        }

        protected void FullUpdateTarget()
        {
            MSG_UNIT_TARGET_FULL_SC response = new MSG_UNIT_TARGET_FULL_SC();
            if (m_target == null)
            {
                response.PercentHP = 0f;
                response.TargetName = "";
            }
            else

            {
                m_target.ReadData(out HealtData healtData);
                m_target.ReadData(out UnitName unitName);
                response.TargetObjectID = m_target.ID;
                response.TargetName = string.IsNullOrEmpty(unitName.Name) ? "null" : unitName.Name;
                response.PercentHP = (healtData.HP / (float)healtData.MaxHP) * 100f;
                m_lastSyncHealtData = healtData;
            }
            //response.TargetName = m_target.Name;
            m_networkTransmission.Socket.Send(response);
        }

        public override void LateUpdate()
        {
            if (m_target == null) return;

            m_target.ReadData(out HealtData healtData);
            if (m_lastSyncHealtData.HP != healtData.HP || m_lastSyncHealtData.MaxHP != healtData.MaxHP)
            {
                HPUpdateTarget();
                return;
            }
        }
    }
}
