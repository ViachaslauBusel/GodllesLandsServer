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
        private bool _isSynced;

        public bool IsDirty => _isDirty;
        public StatField Data => _statField;

        public GameStatField(StatCode type, int value, bool isSynced = true)
        {
            _statField = new StatField
            {
                Code = type,
                Value = value
            };
            _isDirty = _isSynced = isSynced;
        }

        internal void MarkDataAsSynced()
        {
            _isDirty = false;
        }

        internal void SetValue(int value)
        {
            _statField.Value = value;
            _isDirty = _isSynced;
        }
    }
}
