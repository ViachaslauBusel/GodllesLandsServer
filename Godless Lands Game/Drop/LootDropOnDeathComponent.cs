using Game.GameObjectFactory;
using Game.GridMap.Scripts;
using Game.Items;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Systems.Target.Commands;
using Game.Tools;
using Game.UnitVisualization;
using Godless_Lands_Game.UnitVisualization;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
using Zenject;

namespace Game.Drop
{
    public class LootDropOnDeathComponent : Component
    {
        private TransformComponent _transform;
        private BodyComponent _bodyComponent;
        private TargetedUnitTrackerComponent _targetedUnitTracker;
        private ItemsFactory _itemsFactory;
        private ICachedViewComponent _cachedView;
        private bool _deathAndCreateCorpseScriptInProgress;

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
            _cachedView = GetComponent<ICachedViewComponent>();

            if(_cachedView == null)
            {
                Debug.Log.Error($"Can't create loot drop on death for monster {GameObject.ID} without MonsterViewComponent");
                return;
            }

            _bodyComponent.OnDeath += OnDeath;
        }


        private void OnDeath()
        {
            DeathAndCreateCorpseScript();
        }

        private async void DeathAndCreateCorpseScript()
        {
            if (_deathAndCreateCorpseScriptInProgress) return;
            _deathAndCreateCorpseScriptInProgress = true;

            // Set the view as in need of cache visual
            _cachedView.SetInNeedChaceVisual(true);

            // Wait for the next tick to make sure the data is synchronized with the clients
            int currentTick = Time.Tick;
            await new WaitUntil(() => currentTick != Time.Tick);

            // Удалить гейм объект из мира, при удалении визкальный обьект переместиться в кэш откуда он позже будет взят для создания трупа
            GameObject.DestroyComponent<EntityTagComponent>();

            // Set the corpse as the target for all units that were targeting the monster
            foreach (var unit in _targetedUnitTracker.UnitsTargetingThis)
            {
                unit.SendCommand(new SetTargetCommand { GameObjectUnitID = 0 });
            }

            // Подождать пока гейм объект будет удален у клиентов
            currentTick = Time.Tick;
            await new WaitUntil(() => currentTick != Time.Tick);

            _cachedView.SetInNeedChaceVisual(false);

            // Создание трупа
            var dropHolder = CreateDropHolderWithItems();

            GameObject corpse = CorpseFactory.CreateCorpse(_transform.Position, _transform.Rotation, _cachedView.SkinId, _cachedView.CachedObjectId, dropHolder);

            int corpseId = await GameObject.World.AddGameObject(corpse);

           

            _deathAndCreateCorpseScriptInProgress = false;
        }

        private DropHolderComponent CreateDropHolderWithItems()
        {
            var dropHolder = new DropHolderComponent();

            dropHolder.AddItem(_itemsFactory.CreateItem(1, 0, count: RandomHelper.Range(1, 5)));
            dropHolder.AddItem(_itemsFactory.CreateItem(3, 0, count: 1));
            dropHolder.AddItem(_itemsFactory.CreateItem(2, 0, count: 1));
            dropHolder.AddItem(_itemsFactory.CreateItem(12, 0, count: 1));

            return dropHolder;
        }


        public override void OnDestroy()
        {
            _bodyComponent.OnDeath -= OnDeath;
        }
    }
}
