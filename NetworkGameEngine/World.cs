using NetworkGameEngine.Tools;
using System.Collections.Concurrent;

namespace NetworkGameEngine
{
    public class AddingObjectTask : TaskBase
    {
       public GameObject GameObject { get; set; }
        public int GameObjectID { get; set; } = 0;
    }
    public class RemovingObjectTask : TaskBase
    {
        public int GameObjectID { get; set; }
    }
    public class World
    {
        private static Dictionary<int, GameObject> m_objects = new Dictionary<int, GameObject>();
        private static ConcurrentQueue<AddingObjectTask> m_addObjects = new ConcurrentQueue<AddingObjectTask>();
        private static ConcurrentQueue<RemovingObjectTask> m_removeObjects = new ConcurrentQueue<RemovingObjectTask>();
        private static List<GameObject> m_removedObjects = new List<GameObject>();
        private static int m_generatorID = 1;
        private static Workflow[] m_workflows;
        private static int m_addObjectThIndex = 0;
        private static Thread m_mainThread;

        public static void Init(int maxThread)
        {
            m_workflows = new Workflow[maxThread];
            for (int i = 0; i < m_workflows.Length; i++)
            {
                m_workflows[i] = new Workflow();
                m_workflows[i].Init();
            }
            m_mainThread = new Thread(Update);
            m_mainThread.Start();
        }
        public static async Task<int> AddGameObject(GameObject obj)
        {
            var task = new AddingObjectTask() { GameObject = obj };
            m_addObjects.Enqueue(task);
            await task.Wait();

            return task.GameObjectID;
        }

        public static void RemoveGameObject(int gameObjectID) 
        {
            m_removeObjects.Enqueue(new RemovingObjectTask() { GameObjectID = gameObjectID });
        }

        public static void Update()
        {
            while (true)
            {
                int addObjectsCount = m_addObjects.Count;
                for (int i = 0; i < addObjectsCount && m_addObjects.TryDequeue(out var task); i++)
                {
                    GameObject obj = task.GameObject;
                    obj.Init(m_generatorID++, m_workflows[m_addObjectThIndex].ThreadID);

                    m_objects.Add(obj.ID, obj);

                    m_workflows[m_addObjectThIndex].AddObject(obj);
                    m_addObjectThIndex = (m_addObjectThIndex + 1) % m_workflows.Length;

                    task.GameObjectID = obj.ID;
                    task.Completed(true);
                }

                foreach (var th in m_workflows) { th.CallMethod(MethodType.Init); }
                foreach (var th in m_workflows) { th.Wait(); }

                foreach (var th in m_workflows) { th.CallMethod(MethodType.Awake); }
                foreach (var th in m_workflows) { th.Wait(); }

                foreach (var th in m_workflows) { th.CallMethod(MethodType.Start); }
                foreach (var th in m_workflows) { th.Wait(); }

                foreach (var th in m_workflows) { th.CallMethod(MethodType.Update); }
                foreach (var th in m_workflows) { th.Wait(); }

                int removeObjectsCount = m_removeObjects.Count;
                for(int i = 0; i< removeObjectsCount && m_removeObjects.TryDequeue(out var task); i++) 
                {
                    bool isRemoved = m_objects.ContainsKey(task.GameObjectID);
                    if (isRemoved)
                    {
                        GameObject removeObj = m_objects[task.GameObjectID];
                        m_removedObjects.Add(removeObj);
                      
                        removeObj.Destroy();
                    }
                    task.Completed(isRemoved);
                }
                foreach (var th in m_workflows) { th.CallMethod(MethodType.OnDestroy); }
                foreach (var th in m_workflows) { th.Wait(); }

                foreach(var obj in m_removedObjects)
                {
                    m_objects.Remove(obj.ID);
                    m_workflows.First(th => th.ThreadID == obj.ThreadID).RemoveObject(obj);
                }
                m_removedObjects.Clear();   
                Thread.Sleep(100);
            }
        }
    }
}
