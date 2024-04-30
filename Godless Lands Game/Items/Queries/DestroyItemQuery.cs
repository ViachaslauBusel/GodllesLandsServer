using Database;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;
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

        public string Command => $"SELECT destroy_item('{_ownerId}', '{_uniqueId}');";

        public bool IsDone { get; private set; }

        public DestroyItemQuery(int ownerId, long uniqueId)
        {
            _ownerId = ownerId;
            _uniqueId = uniqueId;
        }

        public async Job<bool> Execute()
        {
            try
            {
                bool result = await JobsManager.Execute(GameDatabase.Provider.SelectObject<bool>(Command));
                return result;
            }
            catch (Exception e)
            {
                Debug.Log.Error($"DestroyItemQuery failed: {e.Message}");
                return false;
            }
            finally
            {
                IsDone = true;
            }
        }
    }
}
