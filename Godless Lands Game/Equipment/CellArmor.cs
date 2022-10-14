using Godless_Lands_Game.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Equipment
{
   public class CellArmor
    {
        public ItemType Type { get; private set; }
        public ArmorPart Part { get; private set; }
        public ItemLocation Armor { get; private set; }
        public int ID { get; private set; }

        public CellArmor(ItemType weapon, ArmorPart none, ItemLocation armor)
        {
            Type = weapon;
            Part = none;
            Armor = armor;
        }


    }
}
