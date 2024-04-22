using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Trade.Components
{
    public class TraderHolderComponent : Component
    {
        private Dictionary<int, Trader> _traders = new Dictionary<int, Trader>();
        private Trader _initiator;
        private Trader _acceptor;

        public Trader Initiator  => _initiator;
        public Trader Acceptor => _acceptor;

        public bool IsTradeCancelled => _initiator.IsCancelTrade || _acceptor.IsCancelTrade;
        public IReadOnlyCollection<Trader> Traders => _traders.Values;

        public TraderHolderComponent(GameObject initiator, GameObject acceptor)
        {
            _initiator = new Trader(initiator);
            _acceptor = new Trader(acceptor);

            _traders.Add(_initiator.ID, _initiator);
            _traders.Add(_acceptor.ID, _acceptor);
        }

        internal bool ContainsTrader(int characterObjectID) => _traders.ContainsKey(characterObjectID);

        internal Trader GetTrader(int requesterId)
        {
            if (!_traders.ContainsKey(requesterId))
            {
                return null;
            }
            return _traders[requesterId];
        }
    }
}
