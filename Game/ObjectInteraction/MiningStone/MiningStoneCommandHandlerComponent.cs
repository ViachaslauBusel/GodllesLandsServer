using Game.Drop;
using Game.Inventory.Commands;
using Game.ObjectInteraction.MiningStone.Commands;
using Game.Systems.Stats.Components;
using Godless_Lands_Game.Professions.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.MSG.Game.Professions;

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
                Debug.Log.Warn("MiningStoneCommandHandlerComponent:ReactCommand: Mining stone is already destroyed");
                return;
            }

            _bodyComponent.Kill();

            var addItems = new AddItemToInventoryCommandNoRet()
            {
                Items = _dropHolder.TakeAll()
            };
            command.CharacterObj.SendCommand(addItems);

            var addExperience = new AddExperienceToProfessionCommand(ProfessionType.Mining, 50);
            command.CharacterObj.SendCommand(addExperience);

            _miningStoneInteraction.DisconnectAll();
        }
    }
}
