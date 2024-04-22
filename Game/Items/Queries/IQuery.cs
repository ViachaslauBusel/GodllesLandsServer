using NetworkGameEngine.JobsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items.Queries
{
    public interface IQuery
    {
        string Command { get; }
        Job<bool> Execute();
    }
}
