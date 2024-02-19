using Game.NetworkTransmission;
using Game.Physics.Transform;
using Game.Systems.Stats;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.Data.Replicated;
using Protocol.Data.Replicated.Transform;
using Protocol.MSG.Game.ToClient;
using Protocol.MSG.Game.ToClient.Target;
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

            if(request.GameObjectId == 0)
            {
                m_target = null;
                FullUpdateTarget();
                return;
            }
            bool isSucces = GameObject.World.TryGetGameObject(request.GameObjectId, out m_target);

            if (isSucces == false)
            {
                Debug.Log.Warn($"UnitTargetSelectionComponent: Can't find target with id {request.GameObjectId}");
            }
            FullUpdateTarget();
        }

        private void FullUpdateTarget()
        {
            MSG_UNIT_TARGET_FULL_SC response = new MSG_UNIT_TARGET_FULL_SC();
            if (m_target == null)
            {
                response.PercentHP = -1;
                response.TargetName = "";
            }
            else

            {
                m_target.ReadData(out HealtData healtData);
                m_target.ReadData(out UnitName unitName);
                response.TargetName = unitName.Name;
                response.PercentHP = (int)((healtData.HP / (float)healtData.MaxHP) * 100);
                m_lastSyncHealtData = healtData;
            }
            //response.TargetName = m_target.Name;
            m_networkTransmission.Socket.Send(response);
        }

        private void HPUpdateTarget()
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

        public override void LateUpdate()
        {
            if (m_target == null) return;
            if (m_target.IsDestroyed)
            {
                m_target = null;
                FullUpdateTarget();
                return;
            }
            
            m_target.ReadData(out TransformData targetTransform);
            float distance = Vector3.DistanceSquared(m_transform.Position, targetTransform.Position);

            if(distance > 2_500f)
            {
                m_target = null;
                FullUpdateTarget();
                return;
            }

            m_target.ReadData(out HealtData healtData);
            if (m_lastSyncHealtData.HP != healtData.HP || m_lastSyncHealtData.MaxHP != healtData.MaxHP)
            {
                HPUpdateTarget();
                return;
            }
        }
    }
}
