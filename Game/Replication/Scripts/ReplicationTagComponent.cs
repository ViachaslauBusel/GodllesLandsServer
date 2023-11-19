using Game.Physics;
using NetworkGameEngine;
using NetworkGameEngine.Units.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.Replication.Scripts
{
    internal class ReplicationTagComponent : Component
    {
        private IReplicationService m_replicationService;

        [Inject]
        public void InjectServices(IReplicationService replicationService)
        {
            m_replicationService = replicationService;
            var info = GetComponent<CharacterInfoHolder>();
            //Регистрация обьекта в качества слушателя который будет получать данные обьектов вокруг
            m_replicationService.Register(GameObject.ID, info.Socket);
        }
      

        public override void OnDestroy()
        {
            m_replicationService.Unregister(GameObject.ID);
        }
    }
}
