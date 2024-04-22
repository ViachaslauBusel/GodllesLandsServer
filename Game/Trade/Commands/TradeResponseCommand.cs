using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Trade.Commands
{
    public struct TradeResponseCommand : ICommand
    {
        public GameObject TradeAcceptor { get; set; }
        public bool Result { get; set; }

        public TradeResponseCommand(GameObject tradeAcceptor, bool result)
        {
            TradeAcceptor = tradeAcceptor;
            Result = result;
        }
    }
}
