using Godless_Lands_Game.Equipment;
using Godless_Lands_Game.Physics;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characters
{
    public static class CharacterRW
    {
        public static void WriteCharacter(this Packet packet, Character character)
        {
            packet.WriteInt(character.ID);
            packet.WriteString(character.Stats.Name);
            packet.WriteShort(character.Transform.syncNumber);
            packet.WriteVector3(character.Transform.position);
            packet.WriteFloat(character.Transform.rotation);
            packet.WriteArmor(character.Armor);
            packet.WriteBool(character.CombatState);
        }
    }
}
