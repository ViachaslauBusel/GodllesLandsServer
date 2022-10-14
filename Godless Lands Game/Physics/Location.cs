
using BulletXNA.LinearMath;
using Godless_Lands_Game.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Godless_Lands_Game.Physics
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


        public bool IsEmpty =>   x == -10 || y == -10;

        public override int GetHashCode() => HashCode.Combine(x, y);


        public bool IsCorrectly => x >= 0 && x < World.MaxTiles
                                && y >= 0 && y < World.MaxTiles;


        public static Location CreateLocation(Vector3 position) => new Location()
        {
            x = (int)(position.X / World.tileSize),
            y = (int)(position.Z / World.tileSize)
        };
       
        public static Location CreateEmpty() => new Location(-10, -10);

        public static Location IfChanged(Transform transform)
        {

            double _x = transform.position.X - (transform.location.x * World.tileSize);

            if (_x > 110.0f)
                return new Location(transform.location.x+1, transform.location.y);
            else if(_x < -10.0f)
                return new Location(transform.location.x-1, transform.location.y);

            double _y = transform.position.Z - (transform.location.y * World.tileSize);

            if (_y > 110.0f )
                return new Location(transform.location.x, transform.location.y+1);
            else if(_y < -10.0f)
                return new Location(transform.location.x, transform.location.y-1);


            return CreateEmpty();
        }

        
    public override string ToString()
        {
            return "x: " + x + ", y: " + y + ";";
        }

    }
}