using Game.Physics;
using NetworkGameEngine;
using Zenject;

namespace Game.GridMap.Scripts
{
    /// <summary>
    /// Указывает что обьект содержит данные которые необходимо синхронизировать
    /// </summary>
    internal class MapObjectTagComponent : Component
    {
        private IGridMapService m_mapService;
        private TransformComponent m_transform;
        private ObjectOnMap m_objectOnMap;

        [Inject]
        public void InjectService(IGridMapService mapService)
        {
            m_mapService = mapService;
            //Регистрация обьекта который необходимо синхронизировать
            m_objectOnMap = m_mapService.Register(GameObject);
        }

        public override void Start()
        {
            m_transform = GetComponent<TransformComponent>();
            Location location = new Location((int)(m_transform.Position.X / m_mapService.TileSize), (int)(m_transform.Position.Z / m_mapService.TileSize));
            m_objectOnMap.UpdateLocation(location);
        }

        public override void LateUpdate()
        {
            if(m_objectOnMap.Location.IfChanged(m_transform.Position, m_mapService.TileSize, out Location newLocation))
            {
                m_objectOnMap.UpdateLocation(newLocation);
            }
        }
        
        public override void OnDestroy()
        {
            m_mapService.Unregister(GameObject);
        }
    }
}
