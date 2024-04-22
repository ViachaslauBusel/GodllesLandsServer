using NetworkGameEngine;
using Protocol.MSG.Game.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class TradeInteractionComponent : InteractiveObjectInteractionControllerComponent
    {
        private TraderHolderComponent _traderHolder;


        public override void Init()
        {
            _traderHolder = GetComponent<TraderHolderComponent>();

            OnClientConnected += OnClientConnectedHandler;
            OnClientDisconnected += OnClientDisconnectedHandler;
        }

        private void OnClientDisconnectedHandler(GameObject @object, IPlayerNetworkProfile profile)
        {
            _traderHolder.GetTrader(profile.CharacterObjectID)?.SetProfile(null);
            SetTradeWindowVisibility(profile, false);
        }

        private void OnClientConnectedHandler(GameObject @object, IPlayerNetworkProfile profile, List<Component> components)
        {
            _traderHolder.GetTrader(profile.CharacterObjectID)?.SetProfile(profile);
            components.Add(new CharacterTradeControllerComponent(GameObject));

            SetTradeWindowVisibility(profile, true);
        }

        protected override bool CanStartInteraction(IPlayerNetworkProfile playerProfile)
        {
            return _traderHolder.ContainsTrader(playerProfile.CharacterObjectID);
        }

        private void SetTradeWindowVisibility(IPlayerNetworkProfile profile, bool open)
        {
            // Open trade window
            MSG_OPEN_TRADE_WINDOW_SC msg = new MSG_OPEN_TRADE_WINDOW_SC();
            msg.Visible = open;
            msg.PlayerLock = false;
            msg.PartnerLock = false;
            profile.Client.Send(msg);
        }
    }
}
