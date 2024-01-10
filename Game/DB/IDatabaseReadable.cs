using NetworkGameEngine.JobsManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DB
{
    public interface IDatabaseReadable
    {
          Job ReadFromDatabase();
    }
}
