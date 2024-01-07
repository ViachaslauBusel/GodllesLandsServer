using Game.NetworkTransmission;
using Game.Physics;
using Game.Systems.Stats;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.Data.Replicated.Transform;
using Protocol.MSG.Game.ToClient;
using Protocol.MSG.Game.ToServer;
using RUCP;
using System.Numerics;

namespace Game.Systems.TargetSystem
{
    internal class UnitTargetSelectionComponent : Component
    {
        private NetworkTransmissionComponent m_networkTransmission;
        private TransformComponent m_transform;
        private GameObject m_target = null;
        private HealtData m_lastSyncHealtData;

        public GameObject Target => m_target;

        override public void Start()
        {
            m_transform = GetComponent<TransformComponent>();
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
                m_lastSyncHealtData = healtData;
            }
            //response.TargetName = m_target.Name;
            m_networkTransmission.Socket.Send(response);
        }

        public override void LateUpdate()
        {
            if (m_target == null) return;
            if (m_target.IsDestroyed)
            {
                m_target = null;
                UpdateTarget();
                return;
            }
            
            m_target.ReadData(out TransformData targetTransform);
            float distance = Vector3.DistanceSquared(m_transform.Position, targetTransform.Position);

            if(distance > 2_500f)
            {
                m_target = null;
                UpdateTarget();
                return;
            }

            m_target.ReadData(out HealtData healtData);
            if (m_lastSyncHealtData.HP != healtData.HP || m_lastSyncHealtData.MaxHP != healtData.MaxHP)
            {
                UpdateTarget();
                return;
            }
        }
    }
}
