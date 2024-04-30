using Protocol.Data.Replicated;
using Protocol.MSG.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Replication
{
    internal class ObjectSnapshot
    {
        private int m_gameObjectID;
        private Dictionary<int, IReplicationData> m_replicationData = new Dictionary<int, IReplicationData>();
        private List<IReplicationData> m_updatedReplicationData = new List<IReplicationData>(); 
        private List<int> m_remeovedReplicationData= new List<int>();

        public ObjectSnapshot(int id)
        {
            m_gameObjectID = id;
        }

        internal void StartUpdate()
        {
            m_updatedReplicationData.Clear();
            m_remeovedReplicationData.Clear();

            foreach(var replicationDataID in m_replicationData.Keys) 
            {
                m_remeovedReplicationData.Add(replicationDataID);
            }
            
        }

        internal void UpdateData(IReplicationData replicatedData)
        {
            int dataTypeID = replicatedData.GetID();
            m_remeovedReplicationData.Remove(dataTypeID);

            bool isContainsData = m_replicationData.ContainsKey(dataTypeID);
            if (!isContainsData || m_replicationData[dataTypeID].Version != replicatedData.Version)
            {
                if(!isContainsData)
                {
                    m_replicationData.Add(dataTypeID, replicatedData);
                }
                else 
                {
                    m_replicationData[dataTypeID] = replicatedData;
                }
                m_updatedReplicationData.Add(replicatedData);
            }
        }

        internal bool EndUpdate(out MSG_GAMEOBJECT_UPDATE_SC unitUpdate)
        {
            foreach(var removedDataID in m_remeovedReplicationData) { m_replicationData.Remove(removedDataID); }

            unitUpdate = new MSG_GAMEOBJECT_UPDATE_SC();
            unitUpdate.GameobjectID = m_gameObjectID;
            unitUpdate.UpdatedComponents = m_updatedReplicationData;
            unitUpdate.RemovedComponents = m_remeovedReplicationData;

            return m_updatedReplicationData.Count > 0 || m_remeovedReplicationData.Count > 0;
        }
    }
}
