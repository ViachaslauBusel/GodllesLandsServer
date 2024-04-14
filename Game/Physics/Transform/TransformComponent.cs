using Game.NetworkTransmission;
using Game.PlayerScene;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Replicated.Transform;
using Protocol.MSG.Game.Test;
using System.Numerics;

namespace Game.Physics.Transform
{
    public class TransformComponent : Component, IReadData<TransformData>, IReadData<TransformEvents>
    {
        private NetworkTransmissionComponent _networkTransmission;
        private PlayerSceneControllerComponent m_playerSceneController;
        protected byte m_version = 1;
        private byte m_eventsVersion = 1;
        protected Vector3 m_position;
        protected float m_rotation;
        protected float m_velocity;
        private bool m_inMove;
        private List<TransformEvent> m_eventsForSynchronization = new List<TransformEvent>();
        private List<TransformEvent> m_synchronizedEvents = new List<TransformEvent>();
        private List<Point> m_points = new List<Point>();

        public event Action<Vector3> OnPositionChanged;

        public Vector3 Position => m_position;
        public float Rotation => m_rotation;

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_playerSceneController = GetComponent<PlayerSceneControllerComponent>();
        }

        public byte UpdatePosition(Vector3 position, float rotation, float velocity, bool inMove)
        {
            m_position = position;
            m_rotation = rotation;
            m_velocity = velocity;
            m_inMove = inMove;
            m_version++;
            OnPositionChanged?.Invoke(position);

            //if (_networkTransmission != null)
            //{
            //    m_points.Add(new Point() { Position = position, Color = PointColor.Green });
            //    if (m_points.Count > 20)
            //    {
            //        m_points.RemoveAt(0);
            //    }

            //    MSG_DRAW_POINTS msg = new MSG_DRAW_POINTS();
            //    msg.Points = m_points;
            //    _networkTransmission.Socket.Send(msg);
            //}
            return m_version;
        }
        internal void UpdatePosition(Vector3 position)
        {
            m_position = position;
            m_version++;
            OnPositionChanged?.Invoke(position);
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

        internal void TeleportTo(Vector3 newPostion)
        {
            m_position = newPostion;
            m_version++;
            OnPositionChanged?.Invoke(newPostion);
            m_playerSceneController?.PrepareScene(newPostion);
        }
    }
}
