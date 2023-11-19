﻿using Godless_Lands_Game.Map;
using Godless_Lands_Game.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Godless_Lands_Game.Physics
{
    public struct TileAround: IEnumerable<Tile>, IEnumerator<Tile>
    {
        private Location m_centralLocation;
        private int m_step = 0;
        private Location m_currentLocation;


        public TileAround(Location location)
        {
            m_centralLocation = location;
            m_step = 0;
            m_currentLocation = new Location();
        }

        public Tile Current => World.GetTile(m_currentLocation);
        object IEnumerator.Current => World.GetTile(m_currentLocation);

        private Location NextLocation()
        {
            switch (m_step++) {

                case 0: return new Location(m_centralLocation.x - 1, m_centralLocation.y - 1);
                case 1: return new Location(m_centralLocation.x - 1, m_centralLocation.y);
                case 2: return new Location(m_centralLocation.x - 1, m_centralLocation.y + 1);
                case 3: return new Location(m_centralLocation.x, m_centralLocation.y - 1);
                case 4: return m_centralLocation;
                case 5: return new Location(m_centralLocation.x, m_centralLocation.y + 1);
                case 6: return new Location(m_centralLocation.x + 1, m_centralLocation.y - 1);
                case 7: return new Location(m_centralLocation.x + 1, m_centralLocation.y);
                case 8: return new Location(m_centralLocation.x + 1, m_centralLocation.y + 1);
                default: return Location.CreateEmpty();
            }
        }

     

        public bool MoveNext()
        {
            do
            {
                m_currentLocation = NextLocation();
                if (m_currentLocation.IsCorrectly)
                    return true;
            } while (m_step < 9);
            return false;
        }

        public void Reset()
        {
            m_step = 0;
        }

        public IEnumerator<Tile> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;

        public void Dispose()
        {

        }
    }
}
