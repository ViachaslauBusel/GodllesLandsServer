using Game;
using Game.Systems.Stats.Components;
using NetworkGameEngine;
using Protocol.Data.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Systems.Stats.Components
{
    public class BodyRegenerationComponent : Component
    {
        private BodyComponent _body;
        private float _hp;
        private float _stamina;

        public override void Init()
        {
            _body = GetComponent<BodyComponent>();
        }

        public override void Update()
        {
            _hp += 0.2f * Time.fixedDeltaTimeSeconds;
            _stamina += 1f * Time.fixedDeltaTimeSeconds;

            int hp = (int)_hp;
            if (hp > 0)
            {
                _body.Heal(hp, StatCode.HP, StatCode.MaxHP);
                _hp -= hp;
            }
            int stamina = (int)_stamina;
            if (stamina > 0)
            {
                _body.Heal(stamina, StatCode.Stamina, StatCode.MaxStamina);
                _stamina -= stamina;
            }
        }
    }
}
