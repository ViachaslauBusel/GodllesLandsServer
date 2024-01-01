using Protocol.Data.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Stats
{
    public class GameStatField
    {
        private StatField _statField;
        private bool _isDirty;

        public bool IsDirty => _isDirty;
        public StatField Data => _statField;

        public GameStatField(StatCode type, int value)
        {
            _statField = new StatField
            {
                Code = type,
                Value = value
            };
            _isDirty = true;
        }

        internal void MarkDataAsSynced()
        {
            _isDirty = false;
        }
    }
}
