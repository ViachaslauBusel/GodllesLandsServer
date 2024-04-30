using Game.ObjectInteraction.Commands;
using Godless_Lands_Game.ObjectInteraction.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.MSG.Game.ObjectInteraction;

namespace Godless_Lands_Game.ObjectInteraction
{
    public abstract class InteractiveObjectInteractionControllerComponent : Component, IReactCommandWithResult<InteractionStartRequestCommand, InteractionStartConfirmation>,
                                                                                       IReactCommandWithResult<InteractionEndRequestCommand, bool>
    {
        struct InteractionObjectData
        {
            public readonly GameObject CharacterObj;
            public readonly IPlayerNetworkProfile PlayerNetworkProfile;

            public InteractionObjectData(GameObject characterObj, IPlayerNetworkProfile playerNetworkProfile)
            {
                CharacterObj = characterObj;
                PlayerNetworkProfile = playerNetworkProfile;
            }
        }

        private Dictionary<int, InteractionObjectData> _players = new ();

        public event Action<GameObject, IPlayerNetworkProfile, List<Component>> OnClientConnected;
        public event Action<GameObject, IPlayerNetworkProfile> OnClientDisconnected;

        internal bool IsClientConnected(IPlayerNetworkProfile profile) => _players.ContainsKey(profile.CharacterObjectID);

        public InteractionStartConfirmation ReactCommand(ref InteractionStartRequestCommand command)
        {
            InteractionStartConfirmation interactionStartConfirmation = new InteractionStartConfirmation();
            interactionStartConfirmation.Result = StartInteraction(command.PlayerCharacterObject, command.PlayerProfile, out List<Component> componets);
            interactionStartConfirmation.Components = componets;
            return interactionStartConfirmation;
        }

        private bool StartInteraction(GameObject playerCharacterObj, IPlayerNetworkProfile playerProfile, out List<Component> componets)
        {
            componets = new List<Component>();

            if (playerCharacterObj == null || playerProfile == null || playerCharacterObj.ID != playerProfile.CharacterObjectID)
            {
                Debug.Log.Error("Invalid player object or profile");
                return false;
            }

            if (_players.ContainsKey(playerCharacterObj.ID))
            {
                return false;
            }

            if (CanStartInteraction(playerProfile) == false)
            {
                return false;
            }

            _players.Add(playerCharacterObj.ID, new InteractionObjectData(playerCharacterObj, playerProfile));
            OnClientConnected?.Invoke(playerCharacterObj, playerProfile, componets);

            return true;
        }

        public bool ReactCommand(ref InteractionEndRequestCommand command)
        {
            return StopInteraction(command.PlayerCharacterObjectID);
        }

        private bool StopInteraction(int characterObjId)
        {
            return DisconnectClient(characterObjId);
        }

        protected abstract bool CanStartInteraction(IPlayerNetworkProfile playerProfile);

        public bool DisconnectClient(int characterObjId)
        {
            if (_players.TryGetValue(characterObjId, out var data) == false)
            {
                Debug.Log.Error($"Player {characterObjId} is not connected to {GameObject.ID}");
                return false;
            }

            _players.Remove(characterObjId);

            data.CharacterObj.SendCommand(new InteractionEndNotificationCommand() { ObjectId = GameObject.ID });

            OnClientDisconnected?.Invoke(data.CharacterObj, data.PlayerNetworkProfile);
            return true;
        }

        public override void OnDestroy()
        {
            DisconnectAll();
        }

        internal void DisconnectAll()
        {
            //Disconnect all players
            var players = _players.Keys.ToArray();
            foreach (var charObjId in players)
            {
                DisconnectClient(charObjId);
            }
        }
    }
}
