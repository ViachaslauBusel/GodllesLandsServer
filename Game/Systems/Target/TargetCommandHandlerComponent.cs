using Game.Systems.Target.Commands;
using NetworkGameEngine;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Target
{
    public class TargetCommandHandlerComponent : Component, IReactCommand<SetTargetCommand>
    {
        private TargetManagerComponent _targetManagerComponent;

        public override void Init()
        {
            _targetManagerComponent = GetComponent<TargetManagerComponent>();
        }

        public void ReactCommand(ref SetTargetCommand command)
        {
            GameObject.World.TryGetGameObject(command.GameObjectUnitID, out GameObject target);

            _targetManagerComponent.SetTarget(target);
        }
    }
}
