using Godless_Lands_Game.Characters;
using Godless_Lands_Game.Handler;
using Godless_Lands_Game.Map;
using Godless_Lands_Game.Profiles;
using RUCP;
using RUCP.Handler;
using System;

namespace Godless_Lands_Game.Messenger
{
    public static class Message
    {
        [Handler(Types.ChatMessage)]
        public static void Receiving(Profile profile, Packet pack)
        {
            MsgLayer layer = (MsgLayer)pack.ReadByte();
            string message = pack.ReadString();

            // if (message.Length > 120) message = message.Substring(0, 120);

            Packet packet = Packet.Create(Channel.Reliable);
            packet.OpCode = (Types.ChatMessage);
            packet.WriteByte((byte)layer);
            packet.WriteString(profile.Character.Stats.Name);
            packet.WriteString(message);

            if (layer == MsgLayer.AroundMsg)
                World.SendAround(profile.Character.Transform.location, packet);
            else if (layer == MsgLayer.AllMsg)
                World.SendAll(packet);
        }

        public static void Send(Character character, String message, MsgLayer layer)
        {

            Packet packet = Packet.Create(Channel.Reliable);
            packet.OpCode = (Types.ChatMessage);
            packet.WriteByte((byte)layer);
            packet.WriteString(character.Stats.Name);
            packet.WriteString(message);

            character.Socket.Send(packet);
        }
      /*  public static void systemMessage(Character character, String message, MsgLayer layer, int ... params)
        {
            if (character == null)
            {
                System.out.println("systemMessage: character == null");
                return;
            }
            Packet packet = new Packet(character.socket, Channel.Reliable);
            packet.writeType(Types.ChatMessage);
            packet.writeInt(layer.getValue());
            packet.writeString(character.char_name);
            packet.writeString(message);
            for (int i:params)
            {
                packet.writeInt(i);
            }
            Sender.send(packet);


        }

        public static void systemAraundMessage(Character character, String message, MsgLayer layer, int ... params)
        {
            for (Map _map: new MapEnum(character.getTransform().location))
            {
                for (Character otherClient : _map.players)
                {
                    Packet packet = new Packet(otherClient.socket, Channel.Reliable);
                    packet.writeType(Types.ChatMessage);
                    packet.writeInt(layer.getValue());
                    packet.writeString(character.char_name);
                    packet.writeString(message);
                    for (int i:params)
                    {
                        packet.writeInt(i);
                    }
                    Sender.send(packet);
                }
            }
        }

        public static void systemAraundMessage(Location location, String message, MsgLayer layer, int ... params)
        {
            for (Map _map: new MapEnum(location))
            {
                for (Character otherClient : _map.players)
                {
                    Packet packet = new Packet(otherClient.socket, Channel.Reliable);
                    packet.writeType(Types.ChatMessage);
                    packet.writeInt(layer.getValue());
                    packet.writeString("system");
                    packet.writeString(message);
                    for (int i:params)
                    {
                        packet.writeInt(i);
                    }
                    Sender.send(packet);
                }
            }
        }*/ 
    }
}
