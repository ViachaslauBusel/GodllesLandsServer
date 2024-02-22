﻿using Game.Animation;
using Game.Skills.Commands;
using NetworkGameEngine;
using Protocol.Data.Replicated.Animation;
using Protocol.Data.Stats;

namespace Game.Systems.Stats
{
    public class BodyComponent : Component, IReactCommand<DamageCommand>, IReadData<HealtData>
    {
        private bool _isAlive = true;
        private StatsComponent _stats;
        private AnimatorComponent _animator;

        public bool IsAlive => _isAlive;

        public event Action OnDeath;
        public event Action OnRevive;
        public event Action<GameObject, int> OnDamageReceiving;

        public override void Init()
        {
            _stats = GetComponent<StatsComponent>();
            _animator = GetComponent<AnimatorComponent>();
        }

        public override void Start()
        {
            base.Start();
            _isAlive = _stats.GetStat(StatCode.HP) > 0;
            HandleDeath();
        }

        public void ReactCommand(ref DamageCommand command)
        {
            if (!_isAlive)
            {
                // Character is dead
                return;
            }

            int health = _stats.GetStat(StatCode.HP);
            var damage = CalculateDamage(command);
            ApplyDamage(ref health, damage, command.Attacker);
        }

        public void Revive()
        {
            if (_isAlive)
            {
                return;
            }

            _isAlive = true;
            _animator.SetState(AnimationStateID.Dead, false);
            _stats.SetStat(StatCode.HP, _stats.GetStat(StatCode.MaxHP));
            OnRevive?.Invoke();
        }

        private int CalculateDamage(DamageCommand command)
        {
            var damage = command.PAttack / _stats.GetStat(StatCode.PhysicalDefense);
            return damage < 0 ? 0 : damage;
        }

        private void ApplyDamage(ref int health, int damage, GameObject attacker)
        {
            int realDamage = health;
            health -= damage;
            _isAlive = health > 0;
            HandleDeath();
            health = _isAlive ? health : 0;
            realDamage -= health;
            _stats.SetStat(StatCode.HP, health);
            OnDamageReceiving?.Invoke(attacker, realDamage);
        }

        private void HandleDeath()
        {
            if (_isAlive == false)
            {
                _animator.SetState(AnimationStateID.Dead, true);
                OnDeath?.Invoke();
            }
        }

        public void UpdateData(ref HealtData data)
        {

            data.HP = _stats.GetStat(StatCode.HP);
            data.MaxHP = _stats.GetStat(StatCode.MaxHP);
            data.IsAlive = _isAlive;
        }
    }
}
