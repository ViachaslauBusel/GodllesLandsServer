using NetworkGameEngine.JobsManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DB
{
    public interface IDatabaseWritable
    {
        DatabaseSavePriority DatabaseSavePriority { get; }
        bool HasDataToSave { get; set; }

        Job<bool> WriteToDatabase();
    }
}
