using Game.GridMap;
using Game.Physics;
using Protocol.Data.Replicated;
using Protocol.MSG.Game;
using RUCP;
using System.Collections.Concurrent;

namespace Game.Replication
{
    internal class ReplicationService : IReplicationService
    {
        private ConcurrentQueue<SnapshotStateWorld> m_incomingObjects = new ConcurrentQueue<SnapshotStateWorld>();
        private ConcurrentQueue<int> m_outgoingObjects = new ConcurrentQueue<int>();
        private Dictionary<int, SnapshotStateWorld> m_listeners = new Dictionary<int, SnapshotStateWorld>();
        private IGridMapService m_mapService;

        public ReplicationService(IGridMapService mapService)
        {
            m_mapService = mapService;
        }
        public void Register(int id, Client socket)
        {
            m_incomingObjects.Enqueue(new SnapshotStateWorld(id, socket));
        }

        public void Unregister(int id)
        {
            m_outgoingObjects.Enqueue(id);
        }

        public void Update()
        {
            RemoveOutgoingListeners();
            AddIncomingListeners();

            //Перебираем всех клиентом зарегистрировавшихся на синхронизацию состояния мира
            foreach (SnapshotStateWorld clientSnapshotStore in m_listeners.Values)
            {
                clientSnapshotStore.StartUpdate();
                //Получаем локацию на которой находиться объект игрока, вокруг которого происходит синхронизация
                if (m_mapService.TryGetLocation(clientSnapshotStore.ID, out Location centralLocation))
                {
                    //Перебираем локации вокруг центральной и включая центральную
                    foreach (Location l in new LocationAround(centralLocation))
                    {
                        //Перебираем все объекты на локации
                        Tile tile = m_mapService.GetTile(l);
                        if (tile == null) continue;
                        foreach (var mapObj in tile.Objects)
                        {
                            //Получаем последний снимок состояние этого объекта, который видит этот игрок
                            ObjectSnapshot objectSnapshot = clientSnapshotStore.GetObjectSnapshot(mapObj.ID);
                            objectSnapshot.StartUpdate();
                            //Перебираем все данные(компоненты) объекта, для которых необходима синхронизация
                            foreach (var replicatedData in mapObj.ReadAllData<IReplicationData>())
                            {
                                objectSnapshot.UpdateData(replicatedData);
                            }
                            //true - если данные были обновлены и нуждаются в синхронизации
                            if (objectSnapshot.EndUpdate(out MSG_GAMEOBJECT_UPDATE_SC gameobjectUpdate))
                            {
                                clientSnapshotStore.Socket.Send(gameobjectUpdate);
                            }
                        }
                    }
                }
                //true - если есть обьекты которые больше не находятся в зоне видимости и которые необходима уничтожить на стороне клиента
                if (clientSnapshotStore.EndUpdate(out MSG_GAMEOBJECT_DESTROY_SC destroyedObjects))
                {
                    clientSnapshotStore.Socket.Send(destroyedObjects);
                }
            }
        }

        private void AddIncomingListeners()
        {
            while (m_incomingObjects.TryDequeue(out SnapshotStateWorld snapshot))
            {
                m_listeners.Add(snapshot.ID, snapshot);
            }
        }

        private void RemoveOutgoingListeners()
        {
            while (m_outgoingObjects.TryDequeue(out int removeObj))
            {
                m_listeners.Remove(removeObj);
            }
        }
    }
}
