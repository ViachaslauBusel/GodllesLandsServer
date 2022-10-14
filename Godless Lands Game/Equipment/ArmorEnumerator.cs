using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Equipment
{
    class ArmorEnumerator : IEnumerator<CellArmor>
    {
        private Armor armor;
        private int step = 0;
        private CellArmor cellArmor;

        public ArmorEnumerator(Armor armor)
        {
            this.armor = armor;
        }

        public CellArmor Current => cellArmor;

        object IEnumerator.Current => cellArmor;

        public void Dispose()
        {
         
        }

        public bool MoveNext()
        {
            switch (step++)
            {
                case 0: cellArmor = armor.Weapon;
                    return true;
                case 1:
                    cellArmor = armor.Pickaxe;
                    return true;
                case 2:
                    cellArmor = armor.Head;
                    return true;
                case 3:
                    cellArmor = armor.Body;
                    return true;
                case 4:
                    cellArmor = armor.Hand;
                    return true;
                case 5:
                    cellArmor = armor.Legs;
                    return true;
                case 6:
                    cellArmor = armor.Feet;
                    return true;
                default:
                    return false;
            }
        }

        public void Reset()
        {
            step = 0;
        }
    }
}
