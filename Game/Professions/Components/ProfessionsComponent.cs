using NetworkGameEngine;
using Protocol.MSG.Game.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Professions.Components
{
    internal class ProfessionsComponent : Component
    {
        private Dictionary<ProfessionType, Profession> _professions;

        public IReadOnlyCollection<Profession> Professions => _professions.Values;

        public ProfessionsComponent()
        {
            // Register all professions that are in the ProfessionType enumeration
            _professions = new Dictionary<ProfessionType, Profession>();
            foreach (ProfessionType professionType in Enum.GetValues(typeof(ProfessionType)))
            {
                if (professionType == ProfessionType.None)
                    continue;
                _professions.Add(professionType, new Profession(professionType));
            }
        }

        // Get profession by type
        internal bool TryGetProfession(ProfessionType profession_type, out Profession profession)
        {
            return _professions.TryGetValue(profession_type, out profession);
        }
    }
}
