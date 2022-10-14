using Godless_Lands_Game.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Physics
{
    public class TileLeft : IEnumerator<Tile>, IEnumerable<Tile>
    {
        private Location new_location;
        private Location old_location;
        private Location next_location;
        private int step = 3;
        private Location orentation;


        public TileLeft(Location newLocation, Location oldLocation)
        {
            this.new_location = newLocation;
            this.old_location = oldLocation;

            orentation = new Location(oldLocation.x - newLocation.x, oldLocation.y - newLocation.y);
        }




        public bool MoveNext()
        {
            do
            {

                next_location = NextLocation();
                if (step < 0) return false;
            } while (!next_location.IsCorrectly);
            return true;
        }


        public Tile Current => World.GetTile(next_location);
        object IEnumerator.Current => World.GetTile(next_location);

        private Location NextLocation()
        {
            if (orentation.x != 0)
            {
                return new Location(old_location.x + orentation.x, old_location.y + GetStep());
            }

            if (orentation.y != 0)
            {
                return new Location(old_location.x + GetStep(), old_location.y + orentation.y);
            }

            return Location.CreateEmpty();
        }

        private int GetStep()
        {
            switch (step--)
            {
                case 3:
                    return -1;
                case 2:
                    return 0;
                case 1:
                    return 1;
            }
            return 0;
        }

        public void Reset()
        {
            step = 3;
        }

        public void Dispose()
        {
            
        }

        public IEnumerator<Tile> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
