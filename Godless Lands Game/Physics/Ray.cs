
using BulletXNA.BulletCollision;
using BulletXNA.BulletDynamics;
using BulletXNA.LinearMath;
using Godless_Lands_Game.Map;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Physics
{
    class Ray
    {

        public static Vector3 rayDown(Vector3 currentPosition)
        {
            Vector3 startPoint = currentPosition;
            startPoint.Y = 600.0f;
            Vector3 endPosition = currentPosition;
            endPosition.Y = -600.0f;
            //endPosition.X += 2.0f;
            //endPosition.Z += 2.0f;

            Location location = Location.CreateLocation(currentPosition);
            if (!location.IsCorrectly) return Vector3.Zero;

            DiscreteDynamicsWorld dynamicsWorld = World.GetWorld(location);
            if (dynamicsWorld == null) return Vector3.Zero;


            using (ClosestRayResultCallback callback = new ClosestRayResultCallback(ref startPoint, ref endPosition))
            {
                Debug.Assert(callback != null, "callbacl == null");
                //  callback.Flags = (uint)TriangleRaycastCallback.EFlags.FilterBackfaces;
                //        callback.CollisionFilterGroup = (int)CollisionFilterGroups.StaticFilter;
                //      callback.CollisionFilterMask = (int)CollisionFilterGroups.StaticFilter;
                CollisionWorld collisionWorld = dynamicsWorld.CollisionWorld;
                Debug.Assert(collisionWorld != null, "collisionWorld == null");
                try
                {
                    collisionWorld.RayTest(ref startPoint, ref endPosition, callback);
                }
                catch (NullReferenceException) { return Vector3.Zero; }

                if (callback.HasHit)
                {
                    return callback.m_hitPointWorld;
                }
                return Vector3.Zero;
            }
        }
    }
}
