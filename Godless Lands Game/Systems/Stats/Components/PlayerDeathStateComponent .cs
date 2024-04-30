using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.PlayerDeadState;
using Protocol.MSG.Game.ToClient;
using RUCP;

namespace Game.Systems.Stats.Components
{
    public class PlayerDeathStateComponent : Component
    {
        private BodyComponent _body;
        private NetworkTransmissionComponent _networkTransmission;

        public override void Init()
        {
            _body = GetComponent<BodyComponent>();
            _body.OnDeath += OnDeath;
            _body.OnRevive += OnRevive;

            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _networkTransmission.RegisterHandler(Opcode.MSG_PLAYER_DEATH_STATE, HandleRevivalRequest);
        }

        private void HandleRevivalRequest(Packet packet)
        {
            _body.Revive();
        }

        private void OnRevive()
        {
            _networkTransmission.Socket.Send(new MSG_PLAYER_DEATH_STATE_SC { IsAlive = true });
        }

        private void OnDeath()
        {
            _networkTransmission.Socket.Send(new MSG_PLAYER_DEATH_STATE_SC { IsAlive = false });
        }

        public override void OnDestroy()
        {
            _body.OnDeath -= OnDeath;
            _body.OnRevive -= OnRevive;
            _networkTransmission.UnregisterHandler(Opcode.MSG_PLAYER_DEATH_STATE);
        }
    }
}
