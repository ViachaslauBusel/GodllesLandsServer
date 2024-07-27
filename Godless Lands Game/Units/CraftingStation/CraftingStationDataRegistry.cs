using Game.GameObjectFactory;
using Game.Resources;
using Game.Tools;
using Godless_Lands_Game.GameObjectFactory;
using NetworkGameEngine;
using Protocol.Data.Monsters;
using Protocol.Data.Workbenches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Units.Workbenches
{
    internal static class CraftingStationDataRegistry
    {
        private static Dictionary<int, WorkbenchData> _workbenchData = new Dictionary<int, WorkbenchData>();

        public static void LoadData()
        {
            List<WorkbenchData> workbenches = JsonReader.Read<List<WorkbenchData>>(Path.Combine(ResourceFile.Folder, ResourceFile.Workbenches));

            if (workbenches == null)
            {
                Console.WriteLine("No workbench data found");
                return;
            }

            foreach (WorkbenchData workbench in workbenches)
            {
                _workbenchData.Add(workbench.ID, workbench);
            }
        }
    }
}
