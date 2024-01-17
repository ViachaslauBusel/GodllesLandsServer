using Game.Animation;
using Game.Skills.Commands;
using NetworkGameEngine;
using Protocol.Data.Replicated.Animation;
using Protocol.Data.Stats;

namespace Game.Systems.Stats
{
    public class DamageReceiverComponent : Component, IReactCommand<DamageCommand>
    {
        private StatsComponent _stats;
        private AnimatorComponent _animator;

        public override void Init()
        {
            _stats = GetComponent<StatsComponent>();
            _animator = GetComponent<AnimatorComponent>();
        }

        public void ReactCommand(ref DamageCommand command)
        {
            int health = _stats.GetStat(StatCode.HP);
            if(health == 0)
            {
                // Character is dead
                return;
            }

            var damage = command.PAttack / _stats.GetStat(StatCode.PhysicalDefense);
            if (damage < 0)
            {
                damage = 0;
            }
            
            health -= damage;
            if (health <= 0)
            {
                //TODO : Die
                _animator.SetState(AnimationStateID.Dead, true);
                health = 0;
            }
            _stats.SetStat(StatCode.HP, health);
        }
    }   
}
