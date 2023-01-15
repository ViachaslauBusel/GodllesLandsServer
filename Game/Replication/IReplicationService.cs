using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Replication
{
    public interface IReplicationService
    {
        void Register(GameObject gameObject);

        void Unregister(GameObject gameObject);
    }
}
