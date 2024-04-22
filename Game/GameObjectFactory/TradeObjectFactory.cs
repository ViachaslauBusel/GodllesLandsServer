using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Godless_Lands_Game.ObjectInteraction.Trade;
using Godless_Lands_Game.ObjectInteraction.Trade.Components;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.GameObjectFactory
{
    internal static class TradeObjectFactory
    {
        internal static GameObject CreateTradeObject(GameObject initiator, GameObject acceptor)
        {
            GameObject tradeObject = new GameObject("trade");
            tradeObject.AddComponent(new PlayersNetworkTransmissionComponent());
            tradeObject.AddComponent(new TraderHolderComponent(initiator, acceptor));
            tradeObject.AddComponent(new TradeInteractionComponent());
            tradeObject.AddComponent(new TradeCommandHandlerComponent());
            tradeObject.AddComponent(new TradeClientSyncComponent());
            tradeObject.AddComponent(new TradeListenerComponent());
            tradeObject.AddComponent(new TradeCancellationItemReturnComponent());

            return tradeObject;
        }
    }
}
