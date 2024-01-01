using NetworkGameEngine;
using Protocol.Data.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Stats
{
    public class StatsComponent : Component, IReadData<HealtData>
    {
        protected Dictionary<StatCode, GameStatField> m_stats = new ();

        public StatsComponent()
        {
            m_stats.Add(StatCode.HP, new GameStatField(StatCode.HP, 100));
            m_stats.Add(StatCode.MaxHP, new GameStatField(StatCode.MaxHP, 100));
            m_stats.Add(StatCode.MinPattack, new GameStatField(StatCode.MinPattack, 10));
            m_stats.Add(StatCode.MaxPattack, new GameStatField(StatCode.MaxPattack, 10));
            m_stats.Add(StatCode.PhysicalDefense, new GameStatField(StatCode.PhysicalDefense, 10));
            m_stats.Add(StatCode.AttackSpeed, new GameStatField(StatCode.AttackSpeed, 1));
            m_stats.Add(StatCode.MoveSpeed, new GameStatField(StatCode.MoveSpeed, 1));
        }

        public void UpdateData(ref HealtData data)
        {
            if(m_stats.TryGetValue(StatCode.HP, out var hp))
            {
                data.HP = hp.Data.Value;
            }
            if (m_stats.TryGetValue(StatCode.MaxHP, out var maxHp))
            {
                data.MaxHP = maxHp.Data.Value;
            }
        }

    }
}
