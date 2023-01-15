using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Replication
{
    internal class ReplicationTagComponent : Component
    {
        private IReplicationService m_replicationService;
        public void Construct(IReplicationService replicationService)
        {
            m_replicationService = replicationService;
            m_replicationService.Register(GameObject);
        }
        public override async Task Init()
        {
        }
        public override void Awake()
        {
        }
        public override void Start()
        {
        }
        public override void Update()
        {
        }
        public override void CallReact(ICommand cmd)
        {
        }

      

        public override void OnDestroy()
        {
            m_replicationService.Unregister(GameObject);
        }

      

      
    }
}
