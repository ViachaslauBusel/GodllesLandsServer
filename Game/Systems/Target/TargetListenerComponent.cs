using Game.NetworkTransmission;
using Game.Systems.Target;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.MSG.Game.ToServer;
using RUCP;

namespace Game.Systems.TargetSystem
{
    /// <summary>
    /// This component is listening for netwowk commands from client
    /// </summary>
    internal class TargetListenerComponent : Component
    {
        private NetworkTransmissionComponent m_networkTransmission;
        private TargetManagerComponent m_targetManager;


        public override void Init()
        {
            m_targetManager = GetComponent<TargetManagerComponent>();

            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_networkTransmission.RegisterHandler(Opcode.MSG_UNIT_TARGET_REQUEST_CS, TargetRequestProcces);
        }

        private void TargetRequestProcces(Packet packet)
        {
            packet.Read(out MSG_UNIT_TARGET_REQUEST_CS request);

            if (request.GameObjectId == 0)
            {
                m_targetManager.SetTarget(null);
                return;
            }
            bool isSucces = GameObject.World.TryGetGameObject(request.GameObjectId, out GameObject target);

            if (isSucces == false)
            {
                Debug.Log.Warn($"UnitTargetSelectionComponent: Can't find target with id {request.GameObjectId}");
            }
            m_targetManager.SetTarget(target);
        }
    }
}
