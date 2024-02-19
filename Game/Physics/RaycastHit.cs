using BepuPhysics.Collidables;
using BepuPhysics.Trees;
using BepuPhysics;
using NetworkGameEngine;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Game.Physics
{
    public struct RaycastHit : IRayHitHandler
    {
        public bool hit;
        public float distance;
        public Vector3 point;
        private PhysicalTile m_map;
        private LayerMask m_layers;

        public RaycastHit(PhysicalTile map, LayerMask layers)
        {
            hit = false;
            distance = float.MaxValue;
            point = Vector3.Zero;
            m_map = map;
            m_layers = layers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable)
        {
            return CollisionFilter.AllowCollision(m_map.GetCollisionFilter(collidable), m_layers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable, int childIndex)
        {
            return true;
        }

        /// <summary>
        /// Вызывается со всеми объектами с которыми пересекся луч
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRayHit(in RayData ray, ref float maximumT, float t, in Vector3 normal, CollidableReference collidable, int childIndex)
        {
            //Укоротить луч
            maximumT = t;

            if (t < distance)
            {
                distance = t;
                hit = true;
            }
        }
    }
    public static class Ray
    {

       


    }
}