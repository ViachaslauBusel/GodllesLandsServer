using NetworkGameEngine;
using Protocol.Data.Items.Network;
using Protocol.MSG.Game.Trade;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class TradeClientSyncComponent : Component
    {
        private TradeCommandHandlerComponent _tradeStorage;
        private TradeInteractionComponent _tradeInteraction;
        private TraderHolderComponent _traderHolder;
        private List<IPlayerNetworkProfile> _traders = new List<IPlayerNetworkProfile>();

        public override void Init()
        {
            _tradeStorage = GetComponent<TradeCommandHandlerComponent>();
            _tradeInteraction = GetComponent<TradeInteractionComponent>();
            _traderHolder = GetComponent<TraderHolderComponent>();

            _tradeInteraction.OnClientConnected += OnClientConnected;
            _tradeInteraction.OnClientDisconnected += OnClientDisconnected;
            _tradeStorage.OnTradeStorageChanged += OnTradeStorageChanged;
        }

        private void OnTradeStorageChanged(TradeCell[] bag, int characterId)
        {
            List<ItemSyncData> data = bag.Where(c => c.IsDataSyncWithClientPending).Select(c => FromItemToSyncData(c)).ToList();
            foreach (var cell in bag)
            {
                cell.MarkDataAsSyncedWithClient();
            }
            foreach (var trader in _traders)
            {
                MSG_SYNC_TRADE_WINDOW_SC msg = new MSG_SYNC_TRADE_WINDOW_SC();
                msg.IsOwner = trader.CharacterObjectID == characterId;
                msg.BagSize = bag.Length;
                msg.Items = data;
                trader.Client.Send(msg);
            }
        }

        private void SendFullStorageData(IPlayerNetworkProfile profile)
        {
            SendFullStorage(_traderHolder.Initiator.TradeCells, profile, _traderHolder.Initiator.ID == profile.CharacterObjectID);
            SendFullStorage(_traderHolder.Acceptor.TradeCells, profile, _traderHolder.Acceptor.ID == profile.CharacterObjectID);
        }

        private void SendFullStorage(IReadOnlyCollection<TradeCell> initiatorItems, IPlayerNetworkProfile profile, bool isOwner)
        {
            MSG_SYNC_TRADE_WINDOW_SC msg = new MSG_SYNC_TRADE_WINDOW_SC();
            msg.IsOwner = isOwner;
            msg.BagSize = initiatorItems.Count;
            msg.Items = initiatorItems.Select(c => FromItemToSyncData(c)).ToList();
            profile.Client.Send(msg);
        }

        private ItemSyncData FromItemToSyncData(TradeCell cell)
        {
            if (cell.IsEmpty) return new ItemSyncData()
            {
                UniqueID = 0,
                ItemID = 0,
                Count = 0,
                SlotIndex = cell.SlotIndex
            };

            return new ItemSyncData()
            {
                UniqueID = cell.Item.UniqueID,
                ItemID = cell.Item.Data.ID,
                Count = cell.Item.Count,
                SlotIndex = cell.SlotIndex
            };
        }

        private void OnClientDisconnected(GameObject @object, IPlayerNetworkProfile profile)
        {
            _traders.Remove(profile);
        }

        private void OnClientConnected(GameObject @object, IPlayerNetworkProfile profile, List<Component> list)
        {
            _traders.Add(profile);
            SendFullStorageData(profile);
        }
    }
}
