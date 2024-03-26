using Game.Drop;
using Game.Inventory.Commands;
using Game.ObjectInteraction.MiningStone.Commands;
using Game.Systems.Stats.Components;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ObjectInteraction.MiningStone
{
    public class MiningStoneCommandHandlerComponent : Component, IReactCommand<MiningCompletionCommand>
    {
        private DropHolderComponent _dropHolder;
        private BodyComponent _bodyComponent;
        private MiningStoneInteractionComponent _miningStoneInteraction;

        public override void Init()
        {
            _dropHolder = GetComponent<DropHolderComponent>();
            _bodyComponent = GetComponent<BodyComponent>();
            _miningStoneInteraction = GetComponent<MiningStoneInteractionComponent>();
        }

        public void ReactCommand(ref MiningCompletionCommand command)
        {
            if (_bodyComponent.IsAlive == false)
            {
                Debug.Log.Warn("MiningStoneCommandHandlerComponent:ReactCommand: Mining stone is dead");
                return;
            }

            _bodyComponent.Kill();

            var addItems = new AddItemToInventoryCommandNoRet()
            {
                Items = _dropHolder.TakeAll()
            };
            command.CharacterObj.SendCommand(addItems);

            _miningStoneInteraction.DisconnectAll();
        }
    }
}
