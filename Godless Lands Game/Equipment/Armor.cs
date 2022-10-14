using Godless_Lands_Game.Characters;
using Godless_Lands_Game.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Equipment
{
   public class Armor: IEnumerable<CellArmor>
    {
        private Character character;

        public CellArmor Weapon { get; private set; }
        public CellArmor Pickaxe { get; private set; }
        public CellArmor Head { get; private set; }
        public CellArmor Body { get; private set; }
        public CellArmor Hand { get; private set; }
        public CellArmor Legs { get; private set; }
        public CellArmor Feet { get; private set; }

        public Armor(Character character)
        {
            this.character = character;
        }

        public void Load()
        {
            Weapon = new CellArmor(ItemType.Weapon, ArmorPart.None, ItemLocation.Armor);
            Pickaxe = new CellArmor(ItemType.Pickaxe, ArmorPart.None, ItemLocation.Armor);
            Head = new CellArmor(ItemType.Armor, ArmorPart.Head, ItemLocation.Armor);
            Body = new CellArmor(ItemType.Armor, ArmorPart.Body, ItemLocation.Armor);
            Hand = new CellArmor(ItemType.Armor, ArmorPart.Hand, ItemLocation.Armor);
            Legs = new CellArmor(ItemType.Armor, ArmorPart.Legs, ItemLocation.Armor);
            Feet = new CellArmor(ItemType.Armor, ArmorPart.Feet, ItemLocation.Armor);
            // ItemsTable.loadItems(character, ItemLocation.Armor);

            //   ArmorSynchronizer.update(character, this);
        }

     /*   public void Load(Item item)
        {
            /*    if (item.getDurability() < 1) { character.getInventory().put(false, item); return; }

                synchronized(this) {
                    switch (item.type)
                    {
                        case Weapon:
                            item = weapon.load(item);
                            break;
                        case Pickaxe:
                            item = pickaxe.load(item);
                            break;
                        case Armor:
                            ArmorItem armorItem = (ArmorItem)item;
                            if (armorItem == null) break;
                            switch (armorItem.part)
                            {
                                case Head:
                                    item = head.load(item);
                                    break;
                                case Body:
                                    item = body.load(item);
                                    break;
                                case Hand:
                                    item = hand.load(item);
                                    break;
                                case Legs:
                                    item = legs.load(item);
                                    break;
                                case Feet:
                                    item = feet.load(item);
                                    break;
                            }
                            break;
                    }
                }
                character.getInventory().put(false, item);*/
   //     }

        //Надеть вещь
        public Item dress(Item item)
        {
            /*   if (item.getDurability() < 1) return item;
               synchronized(this) {
                   switch (item.type)
                   {
                       case Weapon:
                           item = weapon.dress(item);
                           break;
                       case Pickaxe:
                           item = pickaxe.dress(item);
                           break;
                       case Armor:
                           ArmorItem armorItem = (ArmorItem)item;
                           if (armorItem == null) break;
                           switch (armorItem.part)
                           {
                               case Head:
                                   item = head.dress(item);
                                   break;
                               case Body:
                                   item = body.dress(item);
                                   break;
                               case Hand:
                                   item = hand.dress(item);
                                   break;
                               case Legs:
                                   item = legs.dress(item);
                                   break;
                               case Feet:
                                   item = feet.dress(item);
                                   break;
                           }
                           break;
                   }
               }
               ArmorSynchronizer.update(character, this);
               character.getStats().calculete();
               return item;*/
            return null;
        }

        public void destroy()
        {
            character = null;
        }
        public void save()
        {
            //  weapon.save();
            //  pickaxe.save();
        }

        public IEnumerator<CellArmor> GetEnumerator()
        {
            return new ArmorEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ArmorEnumerator(this);
        }

        /*     public synchronized void deteriorationWeapon()
             {
                 if (!weapon.empty())
                 {
                     Item item = weapon.item();
                     item.setDurability(item.getDurability() - 1);
                     if (item.getDurability() < 1)
                     {
                         item.setDurability(0);
                         character.getInventory().put(false, takeOffByObjectID(weapon.item().getObject_id()));
                     }
                     InventorySynchronizer.UpdateItem(character, item);
                 }
             }


             /*  public boolean containsKey(int key){
                   if(weapon.item != null && weapon.item.object_id == key) return true;
                   if(pickaxe.item != null && pickaxe.item.object_id == key) return true;
                   return false;
               }*/

        /*  */

        /*  public synchronized Item takeOffByObjectID(int objectID)
          {
              for (CellArmor cell: this)
              {
                  if (cell.containsObjectID(objectID))
                  {
                      Item item = cell.takeItem();
                      ArmorSynchronizer.update(character, this);
                      character.getStats().calculete();
                      return item;
                  }

              }
              return null;
          }
          public synchronized Item takeOff(ItemType type, ArmorPart part)
          {

              for (CellArmor cell: this)
              {
                  if (cell.getType() == type && cell.getPart() == part)
                  {
                      Item item = cell.takeItem();
                      ArmorSynchronizer.update(character, this);
                      character.getStats().calculete();
                      return item;
                  }
              }
              return null;
          }


          @Override
      public Iterator<CellArmor> iterator()
          {
              return new ArmorIterator(this);
          }
      }*/
    }
}
