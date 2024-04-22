using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Trade.Commands
{
    public struct TradeRequestCommand : ICommand
    {
        private GameObject _initiatorObject;
        private string _initiatorCharacterName;

        public GameObject InitiatorObject => _initiatorObject;
        public string InitiatorCharacterName => _initiatorCharacterName;

        public TradeRequestCommand(GameObject initiatorObject, string initiatorCharacterName)
        {
            _initiatorObject = initiatorObject;
            _initiatorCharacterName = initiatorCharacterName;
        }
    }
}
