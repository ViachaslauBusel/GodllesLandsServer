using Game.Systems.Target.Commands;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.Units.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Systems.Target
{
    public class TargetedUnitTrackerComponent : Component, IReactCommand<MarkUnitAsTargetedByCommand>, IReactCommand<UnmarkUnitAsTargetedByCommand>
    {
        // List of units that have targeted this unit
        private List<int> m_unitsTargetingThis = new();


        public void ReactCommand(ref MarkUnitAsTargetedByCommand command)
        {
            if(m_unitsTargetingThis.Contains(command.GameObjectUnitID))
            {
                Debug.Log.Error($"Unit:{command.GameObjectUnitID} already targeting this unit:{GameObject.ID}");
                return;
            }

            m_unitsTargetingThis.Add(command.GameObjectUnitID);
        }

        public void ReactCommand(ref UnmarkUnitAsTargetedByCommand command)
        {
            if(m_unitsTargetingThis.Contains(command.GameObjectUnitID) == false)
            {
                Debug.Log.Error($"Unit:{command.GameObjectUnitID} is not targeting this unit:{GameObject.ID}");
                return;
            }

            m_unitsTargetingThis.Remove(command.GameObjectUnitID);
        }
    }
}
