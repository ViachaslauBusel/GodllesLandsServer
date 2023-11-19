using Game.Commands;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Units.Characters
{
    public class CharacterStatsComponent : Component
    {
        public int HP { get; protected set; } = 1_000;
        public int PhysicalDefense { get; protected set; }


        public override async Task Init()
        {
              
        }

        public override void OnDestroy()
        {
        }

        public void React(DamageCommand command)
        {
            HP -= command.PhysicalDamage;
            int realDamage = 110; 

        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }
    }
}
