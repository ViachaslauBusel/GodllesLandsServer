using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Equipment
{
   public static class ArmorRW
    {
        public static void WriteArmor(this Packet packet, Armor armor)
        {
            foreach (CellArmor cell in armor)
            {
                packet.WriteInt((int)cell.Type);
                packet.WriteInt((int)cell.Part);
                packet.WriteInt(cell.ID);
            }
        }
    }
}
