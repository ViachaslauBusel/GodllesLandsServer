using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics
{
    public struct Location
    {
        public int x;
        public int y;


        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IfChanged(Vector3 position, float tileSize, out Location location)
        {
            location = new Location();

            double _x = position.X - (x * tileSize);

            if (_x > 110.0f)
            {
                location = new Location(x + 1, y);
                return true;
            }
            else if (_x < -10.0f)
            {
                location = new Location(x - 1, y);
                return true;
            }

            double _y = position.Z - (y * tileSize);

            if (_y > 110.0f)
            {
                location = new Location(x, y + 1);
                return true;
            }
            else if (_y < -10.0f)
            {
                location = new Location(x, y - 1);
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "x: " + x + ", y: " + y + ";";
        }

    }
}
