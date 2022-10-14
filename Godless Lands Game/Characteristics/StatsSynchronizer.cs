using Godless_Lands_Game.Handler;
using RUCP;
using RUCP.Client;
using RUCP.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characteristics
{
    class StatsSynchronizer
    {
        public static void Load(ClientSocket socket, Stats stats)
        {
            Packet packet = new Packet(socket, Channel.Reliable);
            packet.WriteType(Types.LoadStats);

            packet.WriteString(stats.Name);
            packet.WriteInt(stats.MinPattack);
            packet.WriteInt(stats.MaxPattack);
            packet.WriteInt(stats.PhysicalDefense);
            packet.WriteFloat(stats.AttackSpeed);
            packet.WriteFloat(stats.MoveSpeed);
            packet.Send();
        }

        public static void Update(ClientSocket socket, Stats stats)
        {
            Packet packet = new Packet(socket, Channel.Discard);
            packet.WriteType(Types.UpdateStats);

            packet.WriteInt(stats.MinPattack);
            packet.WriteInt(stats.MaxPattack);
            packet.WriteInt(stats.PhysicalDefense);
            packet.WriteFloat(stats.AttackSpeed);
            packet.WriteFloat(stats.MoveSpeed);

            packet.Send();
        }
    }
}
