using Game.Animation;
using Game.GridMap.Scripts;
using Game.ObjectInteraction;
using Game.Systems.Stats.Components;
using NetworkGameEngine;
using NetworkGameEngine.JobsSystem;
using Protocol.Data.Replicated.Animation;

namespace Game.Units.Resources
{
    public class ResourceVisibilityToggleComponent : Component
    {
        private BodyComponent _bodyComponent;
        private AnimatorComponent _animator;

        public override void Init()
        {
            _bodyComponent = GetComponent<BodyComponent>();
            _animator = GetComponent<AnimatorComponent>();

            _bodyComponent.OnDeath += OnDeath;
            _bodyComponent.OnRevive += OnRevive;

            OnRevive();
        }

        private void OnRevive()
        {
            GameObject.AddComponent(new InteractiveObjectTagComponent());
            GameObject.AddComponent(new EntityTagComponent());
        }

        private async void OnDeath()
        {
            _animator.SetState(AnimationStateID.Dead, true);
            GameObject.DestroyComponent<InteractiveObjectTagComponent>();

            await new MillisDelayJob(5_000);

            if (_bodyComponent.IsAlive == false) GameObject.DestroyComponent<EntityTagComponent>();
            _animator.SetState(AnimationStateID.Dead, false);
        }

        public override void OnDestroy()
        {
            _bodyComponent.OnDeath -= OnDeath;
            _bodyComponent.OnRevive -= OnRevive;
        }
    }
}
