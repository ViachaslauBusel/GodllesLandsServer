using Game.GameObjectFactory;
using Game.GridMap.Scripts;
using Game.Items;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Systems.Target.Commands;
using Game.Tools;
using Game.UnitVisualization;
using NetworkGameEngine;
using Zenject;

namespace Game.Drop
{
    public class LootDropOnDeathComponent : Component
    {
        private TransformComponent _transform;
        private BodyComponent _bodyComponent;
        private TargetedUnitTrackerComponent _targetedUnitTracker;
        private ItemsFactory _itemsFactory;
        private IViewComponent _viewComponent;

        [Inject]
        private void InjectServices(ItemsFactory itemsFactory)
        {
            _itemsFactory = itemsFactory;
        }

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

            Item item = _itemsFactory.CreateItem(1, 0, count: RandomHelper.Range(1, 5));
            dropHolder.AddItem(item);
            item = _itemsFactory.CreateItem(3, 0, count: 1);
            dropHolder.AddItem(item);
            item = _itemsFactory.CreateItem(2, 0, count: 1);
            dropHolder.AddItem(item);

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
