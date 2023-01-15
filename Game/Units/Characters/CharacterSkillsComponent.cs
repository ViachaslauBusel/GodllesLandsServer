using Game.Commands;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Units.Characters
{
    internal partial class CharacterSkillsComponent : Component, IReactCommand<UseSkillComand>
    {
        public override void Awake()
        {
        }

        public override async Task Init()
        {
        }

        public override void OnDestroy()
        {
        }

        public void React(UseSkillComand command)
        {
            //I have this skill 
            int myDamage = 100;
            float skillBust = 1.2f;
            int targetID = 1234;
            int myID = 1233;

           if(World.TryGetGameObject(targetID, out var gameObject)) 
           {
                gameObject.SendCommand(new DamageCommand()
                {
                   fromObjID = 1233,
                   PhysicalDamage= myDamage,
                });
           }

        }
      
        public override void Start()
        {
        }

        public override void Update()
        {
        }
    }
}
