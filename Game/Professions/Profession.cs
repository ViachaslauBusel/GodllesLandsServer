using Game.DataSync;
using Protocol.MSG.Game.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Professions
{
    internal class Profession : ClientAndDbSyncElement
    {
        private const int _expForLevelUp = 1000;

        private ProfessionType _professionType;
        private int _level;
        private int _experience;

        public ProfessionType ProfessionType => _professionType;
        public int Level => _level;
        public int Experience => _experience;
        public int ExpForLevelUp => _expForLevelUp;

        public Profession(ProfessionType professionType)
        {
            _professionType = professionType;
            SetDataSyncPendingOnlyWithClient();
        }

        public void LoadDataFromDb(int level, int experience)
        {
            _level = level;
            _experience = experience;

            SetDataSyncPendingOnlyWithClient();
            MarkDataAsSyncedWithDb();
        }

        public void AddExperience(int experience)
        {
            _experience += experience;

            if(_experience >= _expForLevelUp)
            {
                _experience -= _expForLevelUp;
                _level++;
            }

            SetDataSyncPending();
        }
    }
}
