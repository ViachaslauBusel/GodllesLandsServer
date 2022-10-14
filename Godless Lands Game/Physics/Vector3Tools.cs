
using BulletXNA.LinearMath;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Godless_Lands_Game.Physics
{
   public static class Vector3Tools
    {

        public static Vector3 ClearY(this Vector3 vector)
        {
            vector.Y = 0.0f;
            return vector;
        }
    }
}
