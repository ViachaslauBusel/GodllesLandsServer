using Game.NetworkTransmission;
using Game.Physics;
using NetworkGameEngine;
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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.PlayerScene
{
    public class PlayerSceneStatusComponent : Component
    {
        private NetworkTransmissionComponent m_networkTransmission;
        private PlayerSceneStatus m_status = PlayerSceneStatus.None;

        public PlayerSceneStatus Status => m_status;

        public event Action<PlayerSceneStatus> OnChangeStatus;

        override public void Start()
        {
            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_networkTransmission.RegisterHandler(Opcode.MSG_SCENE_STATUS, OnPlayerSceneStatus);

            PrepareScene(GetComponent<TransformComponent>().Position);
        }

        public void PrepareScene(Vector3 point)
        {
            m_status = PlayerSceneStatus.Loading;
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
    }
}
