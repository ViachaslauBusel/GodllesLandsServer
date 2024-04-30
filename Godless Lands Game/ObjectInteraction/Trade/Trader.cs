using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction.Trade
{
    public class Trader
    {
        private readonly int _id;
        private readonly  GameObject _gameObject;
        private readonly TradeCell[] _bag;
        private bool _isAcceptTrade = false;
        private bool _isCancelTrade = false;
        private IPlayerNetworkProfile _profile;

        public int ID => _id;
        public TradeCell[] TradeCells => _bag;

        public bool IsAcceptTrade => _isAcceptTrade;
        public bool IsCancelTrade => _isCancelTrade;
        public GameObject GameObject => _gameObject;
        public IPlayerNetworkProfile Profile => _profile;

        public Trader(GameObject obj)
        {
            _id = obj.ID;
            _gameObject = obj;
            _bag = new TradeCell[15];

            for (int i = 0; i < _bag.Length; i++)
            {
                _bag[i] = new TradeCell(i);
            }
        }


        public void SetProfile(IPlayerNetworkProfile profile)
        {
            _profile = profile;
        }

        internal void AcceptTrade()
        {
            _isAcceptTrade = true;
        }

        internal void CancelTrade()
        {
           _isCancelTrade = true;
        }
    }
}
