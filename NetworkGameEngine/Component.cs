using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGameEngine
{
    public abstract class Component
    {
        private GameObject m_gameObject;

        internal void InternalInit(GameObject obj)
        {
          m_gameObject = obj;
        }
        public abstract Task Init();
        public abstract void Awake();
        public abstract void Start();
        public abstract void Update();
        public abstract void OnDestroy();

        protected T GetComponent<T>() where T : Component => m_gameObject.GetComponent<T>();
    }
}
