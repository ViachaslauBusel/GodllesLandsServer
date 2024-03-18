using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DataSync
{
    public class ClientAndDbSyncElement
    {
        private bool _isDataSyncWithClientPending;
        private bool _isDataSyncWithDbPending;

        public bool IsDataSyncWithClientPending => _isDataSyncWithClientPending;
        public bool IsDataSyncWithDbPending => _isDataSyncWithDbPending;


        protected void SetDataSyncPendingOnlyWithClient()
        {
            _isDataSyncWithClientPending = true;
        }

        protected void SetDataSyncPending()
        {
            _isDataSyncWithClientPending = true;
            _isDataSyncWithDbPending = true;
        }

        internal void MarkDataAsSyncedWithClient()
        {
            _isDataSyncWithClientPending = false;
        }

        internal void MarkDataAsSyncedWithDb()
        {
            _isDataSyncWithDbPending = false;
        }
    }
}
