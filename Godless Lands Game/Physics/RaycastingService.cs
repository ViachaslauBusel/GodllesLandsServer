using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics
{
    public class RaycastingService
    {
        private PhysicWorld _physicWorld;


        public RaycastingService(PhysicWorld physicWorld)
        {
            _physicWorld = physicWorld;
        }
        /// <summary>
        /// Casts a ray from start along direction to distance, returns true and the point of collision hit on collision
        /// </summary>
        /// <param name="start"></param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <param name="hitInfo"></param>
        /// <returns></returns>
        public bool Raycast(Vector3 start, Vector3 direction, out RaycastHit hitInfo, float distance, LayerMask layers = LayerMask.ALL)
        {
            PhysicalTile tile = _physicWorld.GetTile(start);
            hitInfo = new RaycastHit(tile, layers);
            if (tile == null) 
            { 
                Debug.Log.Error("Raycast: could not find PhysicalTile");
                return false;
            }

            tile.RayCast(start, direction, distance, ref hitInfo);

            if (hitInfo.hit)
            {
                hitInfo.point = start + (direction * hitInfo.distance);
            }
            return hitInfo.hit;
        }

        public unsafe bool GetTerrainPoint(Vector3 position, out Vector3 terrainPoint)
        {
            terrainPoint = position;
            Vector3 startPoint = position;
            startPoint.Y = 1000.0f;
            Vector3 direction = new Vector3(0.0f, -1.0f, 0.0f);

            PhysicalTile tile = _physicWorld.GetTile(position);
            if (tile == null)
            {
                Debug.Log.Error("Raycast: could not find PhysicalTile");
                return false;
            }

            RaycastHit ray = new RaycastHit(tile, LayerMask.Ground);

            tile.RayCast(startPoint, direction, 2000, ref ray);


            if (ray.hit)
            {
                terrainPoint = startPoint + (direction * ray.distance);
            }

            return ray.hit;
        }
    }
}
