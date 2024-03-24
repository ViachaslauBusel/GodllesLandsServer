using Protocol.Data.SpawnPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.RespawnPoints
{
    public class RespawnPointsService
    {
        internal Vector3 GetNeareatPoint(Vector3 position)
        {
            float minDistance = float.MaxValue;
            Vector3 nearestPoint = Vector3.Zero;

            foreach (var point in RespawnPointsStore.RespawnPoints)
            {
                float distance = Vector3.Distance(position, point.Position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPoint = point.Position;
                }
            }

            return nearestPoint;
        }
    }
}
