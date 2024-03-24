using Game.Resources;
using Newtonsoft.Json;
using Protocol.Data.Items;
using Protocol.Data.SpawnPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.RespawnPoints
{
    internal class RespawnPointsStore
    {
        private static List<RespawnPointData> _respawnPoints = new List<RespawnPointData>();

        public static List<RespawnPointData> RespawnPoints => _respawnPoints;

        public static void Load()
        {
            string fullPath = Path.Combine(ResourceFile.Folder, ResourceFile.RespawnPoints);

            if (File.Exists(fullPath))
            {
                string text = File.ReadAllText(fullPath);
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                _respawnPoints = JsonConvert.DeserializeObject<List<RespawnPointData>>(text, settings);
            }
        }
    }
}
