using Game.Physics.Transform;
using Game.Systems.Stats;
using NetworkGameEngine;
using Protocol.Data.Replicated.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Target
{
    public class TargetManagerComponent : Component
    {
        private TransformComponent m_transform;
        protected GameObject m_target = null;
        protected HealtData m_lastSyncHealtData;

        public GameObject Target => m_target;

        override public void Start()
        {
            m_transform = GetComponent<TransformComponent>();
        }

        internal void SetTarget(GameObject target)
        {
            m_target = target;
        }

        public override void LateUpdate()
        {
            if (m_target == null) return;
            if (m_target.IsDestroyed)
            {
                m_target = null;
                FullUpdateTarget();
                return;
            }

            m_target.ReadData(out TransformData targetTransform);
            float distance = Vector3.DistanceSquared(m_transform.Position, targetTransform.Position);

            if (distance > 2_500f)
            {
                m_target = null;
                FullUpdateTarget();
                return;
            }

            m_target.ReadData(out HealtData healtData);
            if (m_lastSyncHealtData.HP != healtData.HP || m_lastSyncHealtData.MaxHP != healtData.MaxHP)
            {
                HPUpdateTarget();
                return;
            }
        }

        protected virtual void HPUpdateTarget()
        {
        }

        protected virtual void FullUpdateTarget()
        {
        }

        
    }
}
