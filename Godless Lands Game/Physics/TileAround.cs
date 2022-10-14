using Godless_Lands_Game.Map;
using Godless_Lands_Game.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Physics
{
    class TileAround: IEnumerable<Tile>, IEnumerator<Tile>
    {
        private Location location;
        private int step = 0;
        private Location current;


        public TileAround(Location location)
        {
            this.location = location;
        }

        public Tile Current => World.GetTile(current);
        object IEnumerator.Current => World.GetTile(current);

        private Location NextLocation()
        {
            switch (step++) {

                case 0: return new Location(location.x - 1, location.y - 1);
                case 1: return new Location(location.x - 1, location.y);
                case 2: return new Location(location.x - 1, location.y + 1);
                case 3: return new Location(location.x, location.y - 1);
                case 4: return location;
                case 5: return new Location(location.x, location.y + 1);
                case 6: return new Location(location.x + 1, location.y - 1);
                case 7: return new Location(location.x + 1, location.y);
                case 8: return new Location(location.x + 1, location.y + 1);
                default: return Location.CreateEmpty();
            }
        }

     

        public bool MoveNext()
        {
            do
            {
                current = NextLocation();
                if (current.IsCorrectly)
                    return true;
            } while (step < 9);
            return false;
        }

        public void Reset()
        {
            step = 0;
        }

        public IEnumerator<Tile> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;

        public void Dispose()
        {

        }
    }
}
