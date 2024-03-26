using Game.ObjectInteraction.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Items.Network;
using Protocol.Data.Replicated.ObjectInteraction;
using Protocol.MSG.Game.Drop;
using Protocol.MSG.Game.ObjectInteraction;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Drop
{
    public class DropInteractionComponent : Component, IReadData<InteractionObjectData>, IReactCommandWithResult<RequestObjectInteractionCommand, bool>
    {
        private DropHolderComponent _dropHolder;
        private List<IPlayerNetworkProfile> _players = new List<IPlayerNetworkProfile>();
        private List<ItemSyncData> _syncData;

        public event Action<IPlayerNetworkProfile> OnClientConnected;
        public event Action<IPlayerNetworkProfile> OnClientDisconnected;

        public override void Init()
        {
            _dropHolder = GetComponent<DropHolderComponent>();
            _dropHolder.OnUpdateDropList += OnUpdateDropList;
        }

        public override void Start()
        {
            UpdateDropList();
        }

        private void UpdateDropList()
        {
            if(_syncData == null) _syncData = new List<ItemSyncData>();
            else _syncData.Clear();

            for (int i = 0; i < _dropHolder.DropList.Count; i++)
            {
                _syncData.Add(new ItemSyncData
                {
                    ItemID = _dropHolder.DropList[i].Data.ID,
                    Count = _dropHolder.DropList[i].Count,
                    SlotIndex = _dropHolder.DropList[i].OwnerID
                });
            }
        }

        private void OnUpdateDropList()
        {
            UpdateDropList();

            foreach (var client in _players)
            {
              SendDropList(client);
            }
        }

        private void SendDropList(IPlayerNetworkProfile playerProfile)
        {
            MSG_DROP_LIST_SYNC_SC dROP_LIST_SYNC_SC = new MSG_DROP_LIST_SYNC_SC
            {
                OpenWindow = true,
                SyncData = _syncData
            };
            playerProfile.Client.Send(dROP_LIST_SYNC_SC);
        }

        public bool ReactCommand(ref RequestObjectInteractionCommand command)
        {
            if (_players.Contains(command.PlayerProfile)) return false;

            _players.Add(command.PlayerProfile);

            SendDropList(command.PlayerProfile);

            OnClientConnected?.Invoke(command.PlayerProfile);

            Debug.Log.Info($"Player {command.PlayerProfile.CharacterObjectID} connected to drop");
            return true;
        }

        public void DisconnectClient(IPlayerNetworkProfile playerProfile)
        {
            Debug.Log.Info($"Player {playerProfile.CharacterObjectID} disconnected from drop");
            if (_players.Remove(playerProfile))
            {
                OnClientDisconnected?.Invoke(playerProfile);
            }

            GameObject.World.TryGetGameObject(playerProfile.CharacterObjectID, out var character);
            if (character != null)
            {
                character.SendCommand(new InteractionEndNotificationCommand() { ObjectId = GameObject.ID });
            }
        }

        public void UpdateData(ref InteractionObjectData data)
        {
            data.Version = 1;
        }

        public override void OnDestroy()
        {
            //Disconnect all players
            for (int i = _players.Count - 1; i >= 0; i--)
            {
                DisconnectClient(_players[i]);
            }
        }
    }
}
