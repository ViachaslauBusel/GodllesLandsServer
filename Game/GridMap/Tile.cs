using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GridMap
{
    internal class Tile
    {
        private List<PlayerEntity> m_players = new List<PlayerEntity>();

        public IEnumerable<PlayerEntity> Players => m_players;

        internal void Add(PlayerEntity player)
        {
            m_players.Add(player);
        }

        internal void Remove(PlayerEntity player)
        {
           m_players.Remove(player);
        }
    }
}
