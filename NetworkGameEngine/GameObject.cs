namespace NetworkGameEngine
{
    public class GameObject
    {
        private int m_threadID = 0;
        private LinkedList<Component> m_components = new LinkedList<Component>();
        private List<Component> m_newComponents = new List<Component>();
        private List<Component> m_removeComponents = new List<Component>();
        private List<Task> m_tasks = new List<Task>();
        public int ID { get; private set; }
        public int ThreadID => m_threadID;
        public bool isInitialized => m_tasks.All(t => t.IsCompleted);

        public void AddComponent(Component component)
        {
           if(m_threadID != 0 && m_threadID != Thread.CurrentThread.ManagedThreadId)
           {
               // Debug.Log.Fatal($"Attempting to add a component to a thread that does not own the object");
           }
            if (m_components.Any(c => component.GetType() == c.GetType()))
            {
              //  Debug.Log.Error($"It is impossible to re-add a component, such a component already exists on the object");
                return;
            }
            m_components.AddLast(component);
            m_newComponents.Add(component);
        }

        public void RemoveComponent<T>() 
        {
            if (m_threadID != 0 && m_threadID != Thread.CurrentThread.ManagedThreadId)
            {
              //  Debug.Log.Fatal($"Attempting to add a component to a thread that does not own the object");
            }
            var component = m_components.First(c => c is T);
            if(component != null) 
            {
               m_removeComponents.Add(component);
               m_components.Remove(component);
            }
        }
        

        internal void CallInit()
        {
          foreach(var c in m_newComponents) { m_tasks.Add(c.Init()); }
        }

        internal void CallAwake()
        {
            foreach (var c in m_newComponents) { c.Awake(); }
        }

        internal void CallStart()
        {
            foreach (var c in m_newComponents) { c.Start(); }
            m_newComponents.Clear();
        }

        internal void CallUpdate()
        {
            foreach (var c in m_components) { c.Update(); }
        }

       
        internal void Init(int objectID, int threadID)
        {
            ID = objectID;
            m_threadID = threadID;
        }

        internal void Destroy()
        {
            m_removeComponents.AddRange(m_components);
            m_components.Clear();
        }

        internal void CallOnDestroy()
        {
            foreach (var c in m_removeComponents) { c.OnDestroy(); }
            m_removeComponents.Clear();
        }

        internal T GetComponent<T>() where T : Component
        {
          return m_components.FirstOrDefault(c => c is T) as T;
        }
    }
}
