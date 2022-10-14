using Godless_Lands_Game.Characteristics;
using Godless_Lands_Game.DatabaseQuery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characters
{
   public class CharacterStats:Stats
    {
        protected Character character;

        public CharacterStats(Character character)
        {
            this.character = character;
        }
        public void Load()
        {
            Name = CharactersTable.GetCharacterName(character.ID);
           // Clear();
           // StatsSynchronizer.load(character);
        }

        private void Clear()
        {
            MinPattack = 10;
            MaxPattack = 20;
            PhysicalDefense = 15;
            AttackSpeed = 1.0f;
            MoveSpeed = 5.0f;
        }
        public void Destroy()
        {
            character = null;
        }



        public void Calculete()
        {

            Clear();
          /*  foreach (CellArmor cell: character.getArmor())
            {
                switch (cell.getType())
                {
                    case Weapon:
                        if (cell.empty()) continue;
                        WeaponItem weaponItem = (WeaponItem)cell.item();

                        minPattack += (int)((weaponItem.minDamage * Equipment.percentQuality[weaponItem.getCount()]) * (weaponItem.getEnchant_level() * Equipment.percentEnchant));
                        maxPattack += (int)((weaponItem.maxDamage * Equipment.percentQuality[weaponItem.getCount()]) * (weaponItem.getEnchant_level() * Equipment.percentEnchant));
                        attack_speed = weaponItem.speed;
                        break;
                }

            }*/


          //  StatsSynchronizer.update(character);
        }

        public void Synchronize()
        {
            StatsSynchronizer.Update(character.Socket, this);
        }
    }
}
