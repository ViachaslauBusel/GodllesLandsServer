using Game.Skills.Commands;
using NetworkGameEngine;
using Protocol.Data.Stats;

namespace Game.Systems.Stats
{
    public class DamageReceiverComponent : Component, IReactCommand<DamageCommand>
    {
        private StatsComponent _stats;

        public override async Task Init()
        {
            _stats = GetComponent<StatsComponent>();
        }

        public void ReactCommand(ref DamageCommand command)
        {
            var damage = command.PAttack / _stats.GetStat(StatCode.PhysicalDefense);
            if (damage < 0)
            {
                damage = 0;
            }

            int health = _stats.GetStat(StatCode.HP);
            health -= damage;
            if (health <= 0)
            {
                //TODO : Die
                health = 0;
            }
            _stats.SetStat(StatCode.HP, health);
        }
    }   
}
