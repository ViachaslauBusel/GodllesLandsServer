using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GridMap
{
    public class Tile
    {
        private readonly HashSet<Entity> _entities = new HashSet<Entity>();
        public IReadOnlyCollection<Entity> Entities => _entities;

        internal void Add(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.Add(entity);
        }

        internal void Remove(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.Remove(entity);
        }
    }
}
