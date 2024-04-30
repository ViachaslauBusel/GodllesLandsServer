using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics
{
    public class PhysicalTile
    {
        public BufferPool BufferPool { get; init; }
        private Simulation m_simulation;
        private CollidableProperty<CollisionFilter> m_collisionFilters;

        public Simulation Simulation => m_simulation;

        internal void RayCast(Vector3 startPoint, Vector3 direction, float distance, ref RaycastHit ray)
        {
            m_simulation.RayCast(startPoint, direction, distance, ref ray);
        }

        /// <summary>
        /// Gets the thread dispatcher available for use by the simulation.
        /// </summary>
        //  public SimpleThreadDispatcher ThreadDispatcher { get; private set; }

        public PhysicalTile()
        {
            BufferPool = new BufferPool();
            m_simulation = Simulation.Create(BufferPool, new NoCollisionCallbacks(), new DemoPoseIntegratorCallbacks(new Vector3(0, 0, 0)), new SolveDescription(8,1));
            m_collisionFilters = new CollidableProperty<CollisionFilter>();
            m_collisionFilters.Initialize(m_simulation);
        }

        public CollisionFilter GetCollisionFilter(CollidableReference collidable)
        {
            return m_collisionFilters[collidable];
        }

        public void Add<TShape>(TShape shape, Vector3 position, Quaternion rotation, LayerMask layer = LayerMask.Default) where TShape : unmanaged, IShape
        {
            var boxIndex = m_simulation.Shapes.Add(shape);

            var staticHandle = m_simulation.Statics.Add(new StaticDescription(position, rotation, boxIndex));

            ref var collision = ref m_collisionFilters.Allocate(staticHandle);
            collision = new CollisionFilter(boxIndex.Index, layer);
        }
    }
}
