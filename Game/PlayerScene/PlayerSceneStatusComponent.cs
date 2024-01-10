using Game.DB;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.Physics;
using Game.Replication.Scripts;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.Data;
using Protocol.Data.Replicated.Transform;
using Protocol.MSG.Game;
using Protocol.MSG.Game.ToClient;
using Protocol.MSG.Game.ToServer;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.PlayerScene
{
    public class PlayerSceneStatusComponent : Component, IReactCommand<DisconnectCommand>
    {
        private NetworkTransmissionComponent m_networkTransmission;
        private DBControlComponent m_dbControl;
        private PlayerSceneStatus m_status = PlayerSceneStatus.LoadingFromDatabase;
        private bool m_shootdownProccess = false;

        public PlayerSceneStatus Status => m_status;

        public event Action<PlayerSceneStatus> OnChangeStatus;

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
            m_status = PlayerSceneStatus.LoadingFromDatabase;
            MSG_PREPARE_SCENE_SC response = new MSG_PREPARE_SCENE_SC();
            response.GameObjectCharacterID = GameObject.ID;
            response.EntryPoint = point;
            m_networkTransmission.Socket.Send(response);
        }

        private void OnPlayerSceneStatus(Packet packet)
        {
            packet.Read(out MSG_SCENE_STATUS answer);

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
            GameObject.World.RemoveGameObject(GameObject.ID);
        }
    }
}
