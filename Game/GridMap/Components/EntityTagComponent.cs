using Game.NetworkTransmission;
using Game.Physics;
using Game.Physics.Transform;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.GridMap.Scripts
{
    public class EntityTagComponent : Component
    {
        protected IGridMapService m_mapService;
        protected TransformComponent m_transform;
        protected Entity m_entityOnMap;

        [Inject]
        public void InjectService(IGridMapService mapService)
        {
            m_mapService = mapService;
        }

        public override void Init()
        {
            m_transform = GetComponent<TransformComponent>();

            m_entityOnMap = CreateEntity();
            //Регистрация обьекта который необходимо синхронизировать
            m_mapService.Register(m_entityOnMap);

            Location location = new Location((int)(m_transform.Position.X / m_mapService.TileSize), (int)(m_transform.Position.Z / m_mapService.TileSize));
            m_entityOnMap.UpdateLocation(location);
        }

        protected virtual Entity CreateEntity()
        {
            return new Entity(GameObject);
        }

        public override void LateUpdate()
        {
            if (m_entityOnMap.Location.IfChanged(m_transform.Position, m_mapService.TileSize, out Location newLocation))
            {
                m_entityOnMap.UpdateLocation(newLocation);
            }
        }

        public override void OnDestroy()
        {
            m_mapService.Unregister(GameObject.ID);
        }
    }
}
