using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.Physics
{
    /// <summary>
    /// Bit masks which control whether different members of a group of objects can collide with each other.
    /// </summary>
    public struct CollisionFilter
    {
        /// <summary>
        /// A mask of 16 bits, each set bit representing a collision group that an object belongs to.
        /// </summary>
        public LayerMask layer;
        /// <summary>
        /// A mask of 16 bits, each set bit representing a collision group that an object can interact with.
        /// </summary>
        public LayerMask allowCollision;
        /// <summary>
        /// Id of the owner of the object. Objects belonging to different groups always collide.
        /// </summary>
        public int ID;



        /// <summary>
        /// Initializes a collision filter that belongs to one specific subgroup and can collide with any other subgroup.
        /// </summary>
        /// <param name="ID">Id of the group that this filter operates within.</param>
        /// <param name="subgroupId">Id of the subgroup to put this collidable into.</param>
        public CollisionFilter(int ID, LayerMask layer)
        {
            this.ID = ID;

            this.layer = layer;
            allowCollision = LayerMask.ALL;
        }

        /// <summary>
        /// Disables a collision between this filter and the specified subgroup.
        /// </summary>
        /// <param name="subgroupId">Subgroup id to disable collision with.</param>
        public void DisableCollision(LayerMask disableLayer)
        {
            allowCollision &= ~disableLayer;
        }



        /// <summary>
        /// Checks if the filters can collide by checking if b's membership can be collided by a's collidable groups.
        /// </summary>
        /// <param name="a">First filter to test.</param>
        /// <param name="b">Second filter to test.</param>
        /// <returns>True if the filters can collide, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllowCollision(in CollisionFilter a, in CollisionFilter b)
        {
            return (a.layer & b.allowCollision) > 0;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllowCollision(in CollisionFilter a, LayerMask layers)
        {
            return (a.layer & layers) > 0;
        }
    }
}
