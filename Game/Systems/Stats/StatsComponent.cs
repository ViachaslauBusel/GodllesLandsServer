using NetworkGameEngine;
using Protocol.Data.Replicated;
using Protocol.Data.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Stats
{
    public class StatsComponent : Component, IReadData<HealtData>, IReadData<UnitName>
    {
        protected string m_name;
        protected Dictionary<StatCode, GameStatField> m_stats = new ();

        public StatsComponent()
        {
            //m_stats.Add(StatCode.Name, "test");
            m_stats.Add(StatCode.HP, new GameStatField(StatCode.HP, 100));
            m_stats.Add(StatCode.MaxHP, new GameStatField(StatCode.MaxHP, 100));
            m_stats.Add(StatCode.MP, new GameStatField(StatCode.MP, 100));
            m_stats.Add(StatCode.MaxMP, new GameStatField(StatCode.MaxMP, 100));
            m_stats.Add(StatCode.Stamina, new GameStatField(StatCode.Stamina, 100));
            m_stats.Add(StatCode.MaxStamina, new GameStatField(StatCode.MaxStamina, 100));
            m_stats.Add(StatCode.MinPattack, new GameStatField(StatCode.MinPattack, 10));
            m_stats.Add(StatCode.MaxPAttack, new GameStatField(StatCode.MaxPAttack, 20));
            m_stats.Add(StatCode.PhysicalDefense, new GameStatField(StatCode.PhysicalDefense, 30));
            m_stats.Add(StatCode.AttackSpeed, new GameStatField(StatCode.AttackSpeed, 1));
            m_stats.Add(StatCode.MoveSpeed, new GameStatField(StatCode.MoveSpeed, 600));

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

        internal int GetStat(StatCode code)
        {
           if(m_stats.TryGetValue(code, out var stat))
            {
                return stat.Data.Value;
            }
            return 0;
        }

        internal void SetStat(StatCode code, int value)
        {
           if(m_stats.TryGetValue(code, out var stat))
            {
                stat.SetValue(value);
            }
        }

        public void UpdateData(ref UnitName data)
        {
            data.Name = m_name;
            data.Version = 1;
        }
    }
}
