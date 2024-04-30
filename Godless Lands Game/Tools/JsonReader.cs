using DataFileProtocol.Skills;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Tools
{
    public static class JsonReader
    {
        public static T Read<T>(string path)
        {
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(text);
            }
            return default(T);
        }
    }
}
