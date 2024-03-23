using Autofac.Features.OwnedInstances;
using Game.GameObjectFactory;
using Game.GridMap.Scripts;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Systems.Target.Commands;
using Game.UnitVisualization;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Drop
{
    public class LootDropOnDeathComponent : Component
    {
        private TransformComponent _transform;
        private BodyComponent _bodyComponent;
        private TargetedUnitTrackerComponent _targetedUnitTracker;
        private IViewComponent _viewComponent;

        public override void Init()
        {
            _transform = GetComponent<TransformComponent>();
            _bodyComponent = GetComponent<BodyComponent>();
            _targetedUnitTracker = GetComponent<TargetedUnitTrackerComponent>();
            _viewComponent = GetComponent<IViewComponent>();

            _bodyComponent.OnDeath += OnDeath;
            _bodyComponent.OnRevive += OnRevive;
        }

        private void OnRevive()
        {
            _viewComponent.SetNeedChaceVisual(false);
        }

        private async void OnDeath()
        {
            GameObject.DestroyComponent<EntityTagComponent>();

            var dropHolder = new DropHolderComponent();
            var viewComponent = _viewComponent.Clone();
            viewComponent.SetVisualObjecId(GameObject.ID);
            _viewComponent.SetNeedChaceVisual(true);
            GameObject corpse = CorpseFactory.CreateCorpse(_transform.Position, viewComponent, dropHolder);

            int corpseId = await GameObject.World.AddGameObject(corpse);

            // Set the corpse as the target for all units that were targeting the monster
            foreach (var unit in _targetedUnitTracker.UnitsTargetingThis)
            {
                unit.SendCommand(new SetTargetCommand { GameObjectUnitID = corpseId });
            }
            // Debug.Log.Info($"Corpse {corpseId} created"); 
        }

        public override void OnDestroy()
        {
            _bodyComponent.OnDeath -= OnDeath;
            _bodyComponent.OnRevive -= OnRevive;
        }
    }
}
