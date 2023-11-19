using NetworkGameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics
{
   public struct LocationAround : IEnumerable<Location>, IEnumerator<Location>
    {
        private Location m_centralLocation;
        private int m_step;
        private Location m_currentLocation;


        public LocationAround(Location location)
        {
            m_centralLocation = location;
            m_step = 0;
            m_currentLocation = new Location();
        }

        public Location Current => m_currentLocation;
        object IEnumerator.Current => m_currentLocation;

        private Location NextLocation()
        {
            switch (m_step)
            {

                case 0: return new Location(m_centralLocation.x - 1, m_centralLocation.y - 1);
                case 1: return new Location(m_centralLocation.x - 1, m_centralLocation.y);
                case 2: return new Location(m_centralLocation.x - 1, m_centralLocation.y + 1);
                case 3: return new Location(m_centralLocation.x, m_centralLocation.y - 1);
                case 4: return m_centralLocation;
                case 5: return new Location(m_centralLocation.x, m_centralLocation.y + 1);
                case 6: return new Location(m_centralLocation.x + 1, m_centralLocation.y - 1);
                case 7: return new Location(m_centralLocation.x + 1, m_centralLocation.y);
                case 8: return new Location(m_centralLocation.x + 1, m_centralLocation.y + 1);
                default: return new Location();
            }
        }



        public bool MoveNext()
        {
                m_currentLocation = NextLocation();
           return m_step++ < 9;
        }

        public void Reset()
        {
            m_step = 0;
        }

        public IEnumerator<Location> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;

        public void Dispose()
        {

        }
    }
}
