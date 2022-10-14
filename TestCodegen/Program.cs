using Protocol;
using RUCP;
using System;
using System.Numerics;
using System.Reflection.Emit;

namespace Protocol.Codegen
{

public enum SomeEnum: short { one, tree }
    public enum SomeEnumTwo  { one, tree }
    [MessagePack(0, RUCP.Channel.Discard)]
public struct Test
{
        public SomeEnum SomeEnum { get; set; }
        public int[] P_values { get; set; }
        public int P_value{ get; set; }
}


    //[MessagePack(0, RUCP.Channel.Discard)]
    //public struct SomePack
    //{

    //}
    //[MessagePack(0, RUCP.Channel.Queue)]
    //public struct NewPack
    //{

    //}
    public static class Opcode{ public const int MSG_AUTHORIZATION_Request = 2; }
    [MessagePack(Opcode.MSG_AUTHORIZATION_Request, Channel.Reliable)]
    public partial struct MSG_AUTHORIZATION_Request
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public short Version { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Packet packet = Packet.Create(Channel.Queue);

            //      NewPack newPack = packet.ReadNewPack();
            packet.Read(out MSG_AUTHORIZATION_Request msg);
            packet.Write(msg);
            Console.ReadLine(); 
        }
    }
}
