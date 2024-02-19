using Database;
using Game.DB;
using Game.Tools;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsManagment;
using NetworkGameEngine.Units.Characters;
using Protocol.Data.Replicated.Transform;
using System.Numerics;

namespace Game.Physics.Transform
{
    public class TransformComponent : Component, IReadData<TransformData>, IReadData<TransformEvents>
    {
        protected byte m_version = 1;
        private byte m_eventsVersion = 1;
        protected Vector3 m_position;
        protected float m_rotation;
        protected float m_velocity;
        private bool m_inMove;
        private List<TransformEvent> m_eventsForSynchronization = new List<TransformEvent>();
        private List<TransformEvent> m_synchronizedEvents = new List<TransformEvent>();


        public Vector3 Position => m_position;


        public bool HasDataToSave { get => true; set { } }

        public byte UpdatePosition(Vector3 position, float rotation, float velocity, bool inMove)
        {
            m_position = position;
            m_rotation = rotation;
            m_velocity = velocity;
            m_inMove = inMove;
            m_version++;
            return m_version;
        }
        internal void UpdatePosition(Vector3 position)
        {
            m_position = position;
            m_version++;
        }

        internal void UpdateRotation(float angle)
        {
            m_rotation = angle;
            m_version++;
        }

        public override void Update()
        {
            if (m_synchronizedEvents.Count > 0)
            {
                m_synchronizedEvents.Clear();
            }
        }

        public void PushEvent(TransformEvent transformEvent)
        {
            m_eventsForSynchronization.Add(transformEvent);
            //m_eventsVersion++;
            Debug.Log.Debug($"PushEvent Jump:{m_eventsVersion}");
        }

        public void UpdateData(ref TransformEvents data)
        {
            m_synchronizedEvents.Clear();
            if (m_eventsForSynchronization.Count > 0)
            {
                m_synchronizedEvents.AddRange(m_eventsForSynchronization);
                m_eventsForSynchronization.Clear();

                data.Version++;
                data.Events = m_synchronizedEvents;
            }
        }

        public void UpdateData(ref TransformData data)
        {
            if (data.Version == m_version) return;
            data.Version = m_version;
            data.Position = m_position;
            data.Rotation = m_rotation;
            data.Velocity = m_velocity;
            data.InMove = m_inMove;
        }

     
    }
}
