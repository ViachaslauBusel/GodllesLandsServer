using Game.NetworkTransmission;
using Game.Systems.Stats;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.MSG.Game.ToClient;
using Protocol.MSG.Game.ToServer;
using RUCP;

namespace Game.Systems.TargetSystem
{
    internal class UnitTargetSelectionComponent : Component
    {
        private NetworkTransmissionComponent m_networkTransmission;
        private GameObject m_target = null;

        override public void Start()
        {
            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_networkTransmission.RegisterHandler(Opcode.MSG_UNIT_TARGET_REQUEST_CS, TargetRequestProcces);
        }

        private void TargetRequestProcces(Packet packet)
        {
            packet.Read(out MSG_UNIT_TARGET_REQUEST_CS request);

            bool isSucces = GameObject.World.TryGetGameObject(request.GameObjectId, out m_target);

            if (isSucces == false)
            {
                Debug.Log.Warn($"UnitTargetSelectionComponent: Can't find target with id {request.GameObjectId}");
            }
            UpdateTarget();
        }

        private void UpdateTarget()
        {
            MSG_UNIT_TARGET_STATE_SC response = new MSG_UNIT_TARGET_STATE_SC();
            if (m_target == null)
            {
                response.HP = 0;
                response.MaxHP = 0;
                response.TargetName = "";
            }
            else

            {
                m_target.ReadData(out HealtData healtData);
                response.TargetName = m_target.ID.ToString();
                response.HP = healtData.HP;
                response.MaxHP = healtData.MaxHP;
            }
            //response.TargetName = m_target.Name;
            m_networkTransmission.Socket.Send(response);
        }
    }
}
