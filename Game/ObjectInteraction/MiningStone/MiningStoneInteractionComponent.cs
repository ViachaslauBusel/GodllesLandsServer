using Cmd.Terminal;
using Game.ObjectInteraction.Commands;
using Game.Systems.Stats.Components;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Replicated.ObjectInteraction;
using Protocol.MSG.Game.ObjectInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction.MiningStone
{
    public class MiningStoneInteractionComponent : Component, IReactCommandWithResult<RequestObjectInteractionCommand, bool>
    {
        private BodyComponent _bodyComponent;
        private List<IPlayerNetworkProfile> _players = new List<IPlayerNetworkProfile>();   

        public override void Init()
        {
            _bodyComponent = GetComponent<BodyComponent>();
        }

        public bool ReactCommand(ref RequestObjectInteractionCommand command)
        {
            switch (command.State)
            {
                case InteractionState.StartInteraction:
                    return StartInteraction(command.PlayerCharacterObject, command.PlayerProfile);
                case InteractionState.EndInteraction:
                        return StopInteraction(command.PlayerProfile);
                    default: return false;
            }
        }

        private bool StopInteraction(IPlayerNetworkProfile playerProfile)
        {
            if (_players.Contains(playerProfile))
            {
                DisconnectClient(playerProfile);
                return true;
            }

            return false;
        }

        private bool StartInteraction(GameObject playerCharacterObj, IPlayerNetworkProfile playerProfile)
        {
            if (_bodyComponent.IsAlive == false)
            {
                return false;
            }

            if (_players.Contains(playerProfile))
            {
                return false;
            }

            _players.Add(playerProfile);

            playerCharacterObj.AddComponent(new CharacterMiningController(GameObject));

            return true;
        }


        private void DisconnectClient(IPlayerNetworkProfile playerProfile)
        {
            Debug.Log.Info($"Player {playerProfile.CharacterObjectID} disconnected from drop");
            if (_players.Remove(playerProfile))
            {
                //OnClientDisconnected?.Invoke(playerProfile);
            }

            GameObject.World.TryGetGameObject(playerProfile.CharacterObjectID, out var character);
            if (character != null)
            {
                character.SendCommand(new InteractionEndNotificationCommand() { ObjectId = GameObject.ID });
            }
        }

        public override void OnDestroy()
        {
           DisconnectAll();
        }

        internal void DisconnectAll()
        {
            //Disconnect all players
            for (int i = _players.Count - 1; i >= 0; i--)
            {
                DisconnectClient(_players[i]);
            }
        }
    }
}
