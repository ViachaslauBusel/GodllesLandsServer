using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items.Queries
{
    internal class DestroyItemQuery : IQuery
    {
        private readonly int _ownerId;
        private readonly long _uniqueId;

        public string Command => $"CALL destroy_item('{_ownerId}', '{_uniqueId}');";

        public DestroyItemQuery(int ownerId, long uniqueId)
        {
            _ownerId = ownerId;
            _uniqueId = uniqueId;
        }
    }
}
