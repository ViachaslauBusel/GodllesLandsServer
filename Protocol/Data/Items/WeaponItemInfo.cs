using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Data.Items
{
    public class WeaponItemInfo : ItemInfo
    {
        public readonly int MinDamage;
        public readonly int MaxDamage;
        public readonly int AttackSpeed;

        public WeaponItemInfo(int id, bool isStackable, int weight, int minDamage, int maxDamage, int attackSpeed) : base(id, isStackable, weight)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            AttackSpeed = attackSpeed;
        }
    }
}
