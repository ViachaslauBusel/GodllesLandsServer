using Game.DB;
using Game.NetworkTransmission;
using Game.Physics.Transform;
using Godless_Lands_Game.WorldEntry;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using Protocol;
using Protocol.Data;
using Protocol.MSG.Game;
using Protocol.MSG.Game.ToClient;
using Protocol.MSG.Game.ToServer;
using RUCP;
using System.Numerics;
using Zenject;

namespace Game.PlayerScene
{
    public class PlayerSceneControllerComponent : Component, IReactCommand<DisconnectCommand>
    {
        private NetworkTransmissionComponent m_networkTransmission;
        private DBControlComponent m_dbControl;
        private PlayerWorldEntryController m_worldEntryController;
        private PlayerSceneStatus m_status = PlayerSceneStatus.LoadingFromDatabase;
        private bool m_shootdownProccess = false;

        public PlayerSceneStatus Status => m_status;

        public event Action<PlayerSceneStatus> OnChangeStatus;

        [Inject]
        private void InjectServices(PlayerWorldEntryController worldEntryController)
        {
            m_worldEntryController = worldEntryController;
        }

        public override void Init()
        {
            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_networkTransmission.RegisterHandler(Opcode.MSG_SCENE_STATUS, OnPlayerSceneStatus);
            m_dbControl = GetComponent<DBControlComponent>();
        }

        override public void Start()
        {
            m_dbControl.OnDatabaseLoadComplete += OnDatabaseLoadComplete;
            m_dbControl.StartLoadFromDatabase();
        }

        private void OnDatabaseLoadComplete()
        {
            PrepareScene(GetComponent<TransformComponent>().Position);
        }

        public void PrepareScene(Vector3 point)
        {
            m_status = PlayerSceneStatus.WaitPrepareClient;
            MSG_PREPARE_SCENE_SC response = new MSG_PREPARE_SCENE_SC();
            response.GameObjectCharacterID = GameObject.ID;
            response.EntryPoint = point;
            m_networkTransmission.Socket.Send(response);
            OnChangeStatus?.Invoke(m_status);
        }

        private void OnPlayerSceneStatus(Packet packet)
        {
            packet.Read(out MSG_SCENE_STATUS answer);

            //Ready for sync
            m_status = answer.Status;

            OnChangeStatus?.Invoke(m_status);
        }

        public override void OnDestroy()
        {
            m_networkTransmission.UnregisterHandler(Opcode.MSG_SCENE_STATUS);
        }

        public void ReactCommand(ref DisconnectCommand command)
        {
            DestroyProccess();
        }

        private async void DestroyProccess()
        {
            if (m_shootdownProccess) return;

            m_shootdownProccess = true;

            //DestroyComponent<MapObjectTagComponent>();
            //DestroyComponent<ReplicationTagComponent>();


            m_status = PlayerSceneStatus.Shutdown;


            bool result = await m_dbControl.Shootdown();

            Debug.Log.Debug($"PlayerSceneStatusComponent DestroyProccess");
            m_worldEntryController.DisconnectedCharacter(GameObject.ID);
            GameObject.World.RemoveGameObject(GameObject.ID);
        }
    }
}
