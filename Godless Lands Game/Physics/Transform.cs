using BulletXNA.LinearMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Physics
{
   public class Transform
    {
        public Vector3 position;
        public Location location;
        public float rotation;
        public short syncNumber = -1;
    }
}
