using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Resources
{
    public static class ResourceReader
    {
        public static void Read(string _fileName, Action<BinaryReader> action)
        {
            try
            {
                string path = Path.Combine(Environment.CurrentDirectory, ResourceFile.Folder, _fileName);

                if (File.Exists(path))
                {
                    using (FileStream file = File.Open(path, FileMode.Open))
                    {
                        using (BinaryReader reader = new BinaryReader(file))
                        {
                            action.Invoke(reader);
                        }
                    }

                }
                else Console.Error.WriteLine($"File {_fileName} no found in path: {path}");
            }
            catch (Exception e) { Console.Error.WriteLine($"File {_fileName} failed to read: {e}"); }
        }
    }
}
