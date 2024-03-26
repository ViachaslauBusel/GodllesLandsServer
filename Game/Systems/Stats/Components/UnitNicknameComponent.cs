using NetworkGameEngine;
using Protocol.Data.Replicated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Stats.Components
{
    public class UnitNicknameComponent : Component, IReadData<UnitName>
    {
        private string m_name;
        public UnitNicknameComponent(string name)
        {
            m_name = name;
        }

        public void UpdateData(ref UnitName data)
        {
            data.Name = m_name;
            data.Version = 1;
        }
    }
}
