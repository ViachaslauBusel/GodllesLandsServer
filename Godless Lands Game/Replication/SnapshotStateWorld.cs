using Protocol.MSG.Game;
using RUCP;

namespace Game.Replication
{
    internal class SnapshotStateWorld
    {
        private int m_id;
        private Client m_socket;
        private Dictionary<int, ObjectSnapshot> m_objects = new Dictionary<int, ObjectSnapshot>();
        private List<int> m_removedObjects = new List<int>();

        public int ID => m_id;
        public Client Socket => m_socket;

        public SnapshotStateWorld(int id, Client socket)
        {
            m_id = id;
            m_socket = socket;
        }

        internal void StartUpdate()
        {
            m_removedObjects.Clear();
            m_removedObjects.AddRange(m_objects.Keys); 
        }

        internal ObjectSnapshot GetObjectSnapshot(int id)
        {
            m_removedObjects.Remove(id);
            if (!m_objects.ContainsKey(id))
            { m_objects.Add(id, new ObjectSnapshot(id)); }

            return m_objects[id];
        }

        internal bool EndUpdate(out MSG_GAMEOBJECT_DESTROY_SC destroyedObjects)
        {
            foreach (var removedObjectID in m_removedObjects) { m_objects.Remove(removedObjectID); }

            destroyedObjects = new MSG_GAMEOBJECT_DESTROY_SC();
            destroyedObjects.DestroyedObjects = m_removedObjects;
            return m_removedObjects.Count > 0;
        }
    }
}