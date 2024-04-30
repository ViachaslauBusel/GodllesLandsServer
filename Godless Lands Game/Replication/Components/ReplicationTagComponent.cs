using Game.NetworkTransmission;
using Game.PlayerScene;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data;
using Zenject;

namespace Game.Replication.Scripts
{
    internal class ReplicationTagComponent : Component
    {
        private NetworkTransmissionComponent m_transmission;
        private PlayerSceneControllerComponent m_playerSceneStatusComponent;
        private IReplicationService m_replicationService;
        private bool m_registered = false;

        [Inject]
        public void InjectServices(IReplicationService replicationService)
        {
            m_replicationService = replicationService;
        }

        override public void Start()
        {
            m_transmission = GetComponent<NetworkTransmissionComponent>();
            m_playerSceneStatusComponent = GetComponent<PlayerSceneControllerComponent>();
            m_playerSceneStatusComponent.OnChangeStatus += OnChangeStatus;
            OnChangeStatus(m_playerSceneStatusComponent.Status);
        }

        private void OnChangeStatus(PlayerSceneStatus status)
        {
            if (status == PlayerSceneStatus.ReadyForSync)
            {
                Register();
            }
            else
            {
                Unregister();
            }
        }

        private void Register()
        {
            if(m_registered) return;

            Debug.Log.Info("ReplicationTagComponent: Register object for replication");
            m_registered = true;
            //Регистрация обьекта в качества слушателя который будет получать данные обьектов вокруг
            m_replicationService.Register(GameObject.ID, m_transmission.Socket);
        }

        private void Unregister()
        {
            if (m_registered == false) return;
            
            Debug.Log.Info("ReplicationTagComponent: Unregister object for replication");
            m_registered = false;
            m_replicationService.Unregister(GameObject.ID);
        }

        public override void OnDestroy()
        {
            m_replicationService.Unregister(GameObject.ID);
        }
    }
}
