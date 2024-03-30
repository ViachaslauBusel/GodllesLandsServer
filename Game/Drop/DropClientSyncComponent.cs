using Game.Drop;
using NetworkGameEngine;
using Protocol.Data.Items.Network;
using Protocol.MSG.Game.Drop;

namespace Godless_Lands_Game.Drop
{
    public class DropClientSyncComponent : Component
    {
        private DropInteractionComponent _dropInteractionComponent;
        private DropHolderComponent _dropHolder;
        private List<ItemSyncData> _syncData;
        private List<IPlayerNetworkProfile> _players = new List<IPlayerNetworkProfile>();

        public override void Init()
        {
            _dropInteractionComponent = GetComponent<DropInteractionComponent>();
            _dropHolder = GetComponent<DropHolderComponent>();

            _dropInteractionComponent.OnClientConnected += OnClientConnectedHandler;
            _dropInteractionComponent.OnClientDisconnected += OnClientDisconnectedHandler;
            _dropHolder.OnUpdateDropList += OnUpdateDropList;
        }

        private void OnClientDisconnectedHandler(GameObject @object, IPlayerNetworkProfile profile)
        {
           _players.Remove(profile);
        }

        private void OnClientConnectedHandler(GameObject @object, IPlayerNetworkProfile profile, List<Component> components)
        {
           _players.Add(profile);
            SendDropList(profile);
        }

        public override void Start()
        {
            UpdateDropList();
        }

        private void UpdateDropList()
        {
            if (_syncData == null) _syncData = new List<ItemSyncData>();
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
    }
}
