using Game.Physics.Transform;
using Game.Systems.Stats;
using Game.Systems.Target.Commands;
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
        private GameObject m_target = null;

        public GameObject Target => m_target;

        public event Action<GameObject> OnTargetChanged;

        override public void Start()
        {
            m_transform = GetComponent<TransformComponent>();
        }

        internal void SetTarget(GameObject target)
        {
            m_target?.SendCommand(new UnmarkUnitAsTargetedByCommand { GameObjectUnitID = GameObject.ID });

            m_target = target;

            m_target?.SendCommand(new MarkUnitAsTargetedByCommand { GameObjectUnitID = GameObject.ID });

            OnTargetChanged?.Invoke(target);
        }

        public override void Update()
        {
            if (m_target == null) return;
            if (m_target.IsDestroyed)
            {
                SetTarget(null);
                return;
            }

            m_target.ReadData(out TransformData targetTransform);
            float distance = Vector3.DistanceSquared(m_transform.Position, targetTransform.Position);

            if (distance > 2_500f)
            {
                SetTarget(null);
                return;
            }
        }
    }
}
