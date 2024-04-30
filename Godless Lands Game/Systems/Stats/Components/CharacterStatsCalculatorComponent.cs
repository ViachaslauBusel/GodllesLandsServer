using Game.Equipment.Components;
using Game.Items;
using NetworkGameEngine;
using Protocol.Data.Items;
using Protocol.Data.Stats;
using Protocol.MSG.Game.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Stats.Components
{
    internal class CharacterStatsCalculatorComponent : Component
    {
        private EquipmentComponent _equipment;
        private StatsComponent _stats;

        public override void Init()
        {
            _equipment = GetComponent<EquipmentComponent>();
            _stats = GetComponent<StatsComponent>();

            _equipment.OnEquipmentChanged += OnEquipmentChanged;
        }

        private void OnEquipmentChanged()
        {
            CalculateStat(StatType.PAttack);   
        }

        public void CalculateStat(StatType type)
        {
            switch (type)
            {
                case StatType.PAttack:
                    CalculatePAttack();
                    break;
                case StatType.MoveSpeed:
                    CalculateMoveSpeed();
                    break;
            }
        }

        private void CalculatePAttack()
        {
            int minPAttack = 10;
            int maxPAttack = 20;
            Item weapon = _equipment.GetItem(EquipmentType.WeaponRightHand);
            if (weapon != null && weapon.Data is WeaponItemData weaponData)
            {
                minPAttack += weaponData.MinDamage;
                maxPAttack += weaponData.MaxDamage;
            }
            _stats.SetStat(StatCode.MinPattack, minPAttack);
            _stats.SetStat(StatCode.MaxPattack, maxPAttack);
        }

        private void CalculateMoveSpeed()
        {
            int moveSpeed = _stats.GetStat(StatCode.BlockMove) > 0 ? 0 : 600;
            
            _stats.SetStat(StatCode.MoveSpeed, moveSpeed);
        }
  
    }
}
