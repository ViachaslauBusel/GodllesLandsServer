using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DataSync
{
    internal class ClientAndDbSyncElementObject : ClientAndDbSyncElement
    {
        public new bool IsDataSyncWithClientPending => base.IsDataSyncWithClientPending;
        public new bool IsDataSyncWithDbPending => base.IsDataSyncWithDbPending;
        public new void MarkDataAsSyncedWithClient() => base.MarkDataAsSyncedWithClient();
        public new void MarkDataAsSyncedWithDb() => base.MarkDataAsSyncedWithDb();
        public new void SetDataSyncPending() => base.SetDataSyncPending();
        public new void SetDataSyncPendingOnlyWithClient() => base.SetDataSyncPendingOnlyWithClient();
    }
}
