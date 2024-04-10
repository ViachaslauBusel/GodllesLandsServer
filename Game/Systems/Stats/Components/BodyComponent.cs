using Game.Animation;
using Game.Physics.Transform;
using Game.RespawnPoints;
using Game.Skills.Commands;
using NetworkGameEngine;
using Protocol.Data.Replicated.Animation;
using Protocol.Data.Stats;
using Zenject;

namespace Game.Systems.Stats.Components
{
    public class BodyComponent : Component, IReactCommandWithResult<DamageCommand, DamageResponse>, IReadData<HealtData>
    {
        private bool _isAlive = true;
        private StatsComponent _stats;
        private AnimatorComponent _animator;
        private TransformComponent _transform;
        private RespawnPointsService _respawnPointsService;

        public bool IsAlive => _isAlive;

        public event Action OnDeath;
        public event Action OnRevive;
        public event Action<GameObject, int> OnDamageReceiving;

        [Inject]
        private void InjectServices(RespawnPointsService respawnPointsService)
        {
            _respawnPointsService = respawnPointsService;
        }

        public override void Init()
        {
            _stats = GetComponent<StatsComponent>();
            _animator = GetComponent<AnimatorComponent>();
            _transform = GetComponent<TransformComponent>();
        }

        public override void Start()
        {
            base.Start();
            _isAlive = _stats.GetStat(StatCode.HP) > 0;
            HandleDeath();
        }

        // Этот метод будет выполнен на этапе Command
        public DamageResponse ReactCommand(ref DamageCommand command)
        {
            if (!_isAlive)
            {
                // Unit is dead
                return new DamageResponse { Damage = 0 };
            }

            int damage = CalculateDamage(command);
            int realDamage = ApplyDamage(damage, command.Attacker);
            return new DamageResponse { Damage = realDamage };
        }

        public void Revive()
        {
            if (_isAlive)
            {
                return;
            }

            _transform.UpdatePosition(_respawnPointsService.GetNeareatPoint(_transform.Position));
            _isAlive = true;
            _animator.SetState(AnimationStateID.Dead, false);
            _stats.SetStat(StatCode.HP, _stats.GetStat(StatCode.MaxHP));
            _stats.SetStat(StatCode.MP, _stats.GetStat(StatCode.MaxMP));
            _stats.SetStat(StatCode.Stamina, _stats.GetStat(StatCode.MaxStamina));
            OnRevive?.Invoke();
        }

        private int CalculateDamage(DamageCommand command)
        {
            var damage = command.PAttack / _stats.GetStat(StatCode.PhysicalDefense);
            return damage < 0 ? 1 : damage;
        }

        private int ApplyDamage(int damage, GameObject attacker)
        {
            int health = _stats.GetStat(StatCode.HP);
            int realDamage = health;
            health -= damage;
            _isAlive = health > 0;
            HandleDeath();
            health = _isAlive ? health : 0;
            realDamage -= health;
            _stats.SetStat(StatCode.HP, health);
            OnDamageReceiving?.Invoke(attacker, realDamage);
            return realDamage;
        }

        public void Heal(int amount)
        {
            if (_isAlive)
            {
                int health = _stats.GetStat(StatCode.HP);
                health += amount;
                int maxHP = _stats.GetStat(StatCode.MaxHP);
                health = health > maxHP ? maxHP : health;
                _stats.SetStat(StatCode.HP, health);
            }
        }

        public void HandleDeath()
        {
            if (_isAlive == false)
            {
                _animator.SetState(AnimationStateID.Dead, true);
                OnDeath?.Invoke();
            }
        }

        public void Kill()
        {
            if (_isAlive)
            {
                _isAlive = false;
                _stats.SetStat(StatCode.HP, 0);
                HandleDeath();
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
