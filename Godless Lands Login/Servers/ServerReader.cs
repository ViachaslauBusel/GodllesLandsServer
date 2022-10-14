using Protocol.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Godless_Lands_Login.Servers
{
    class ServerReader
    {
        private const string file = "servers.txt";
        public static ServerInfo[] Servers { get; private set; }



        public static void Load()
        {
            string path = Path.Combine(Environment.CurrentDirectory, file);
            if (!File.Exists(path)) { Console.WriteLine("Неудалось найти файл с адресами серверов"); return; }
            List<ServerInfo> servers = new List<ServerInfo>();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                   
                    string[] words = sr.ReadLine().Replace(" ", "").Split(":");
                    if (words.Length != 3) continue;
                    try
                    {
                        servers.Add(new ServerInfo() { Name = words[0], IP = words[1], Port = Int32.Parse(words[2]) });
                    } catch { }
                } 
            }
        }
    }
}
