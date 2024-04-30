using NetworkGameEngine;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Replication
{
    public interface IReplicationService
    {
        void Register(int id, Client socket);

        void Unregister(int id);
        void Update();
    }
}
