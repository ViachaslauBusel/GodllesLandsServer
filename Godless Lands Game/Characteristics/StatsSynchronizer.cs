using Godless_Lands_Game.Handler;
using RUCP;
using RUCP;
using RUCP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Characteristics
{
    class StatsSynchronizer
    {
        public static void Load(Client socket, Stats stats)
        {
            Packet packet = Packet.Create(Channel.Reliable);
            packet.OpCode = (Types.LoadStats);

            packet.WriteString(stats.Name);
            packet.WriteInt(stats.MinPattack);
            packet.WriteInt(stats.MaxPattack);
            packet.WriteInt(stats.PhysicalDefense);
            packet.WriteFloat(stats.AttackSpeed);
            packet.WriteFloat(stats.MoveSpeed);
            socket.Send(packet);
        }

        public static void Update(Client socket, Stats stats)
        {
            Packet packet = Packet.Create(Channel.Discard);
            packet.OpCode = (Types.UpdateStats);

            packet.WriteInt(stats.MinPattack);
            packet.WriteInt(stats.MaxPattack);
            packet.WriteInt(stats.PhysicalDefense);
            packet.WriteFloat(stats.AttackSpeed);
            packet.WriteFloat(stats.MoveSpeed);

            socket.Send(packet);
        }
    }
}
