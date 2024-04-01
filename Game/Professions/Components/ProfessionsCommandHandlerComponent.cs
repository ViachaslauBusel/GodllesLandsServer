using Godless_Lands_Game.Professions.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Professions.Components
{
    public class ProfessionsCommandHandlerComponent : Component, IReactCommand<AddExperienceToProfessionCommand>
    {
        private ProfessionsComponent _professions;

        public override void Init()
        {
            _professions = GetComponent<ProfessionsComponent>();
        }

        public void ReactCommand(ref AddExperienceToProfessionCommand command)
        {
            if(_professions.TryGetProfession(command.ProfessionType, out var profession))
            {
                profession.AddExperience(command.Experience);
            }
            else
            {
                Debug.Log.Warn($"ProfessionsCommandHandlerComponent:ReactCommand: Profession {command.ProfessionType} not found");
            }
        }
    }
}
