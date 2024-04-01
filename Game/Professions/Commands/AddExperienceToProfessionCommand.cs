using NetworkGameEngine;
using Protocol.MSG.Game.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Professions.Commands
{
    public struct AddExperienceToProfessionCommand : ICommand
    {
        public readonly ProfessionType ProfessionType;
        public readonly int Experience;

        public AddExperienceToProfessionCommand(ProfessionType professionType, int experience)
        {
            ProfessionType = professionType;
            Experience = experience;
        }
    }
}
