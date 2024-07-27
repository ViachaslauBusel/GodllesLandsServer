using Game.Animation;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Game.ObjectInteraction.MiningStone;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Units.Resources;
using Game.Units.Monsters.Components;
using Game.UnitVisualization;
using Godless_Lands_Game.ObjectInteraction.Workbench;
using Godless_Lands_Game.UnitVisualization;
using Microsoft.VisualBasic;
using NetworkGameEngine;
using Protocol.Data.Workbenches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.GameObjectFactory
{
    internal static class CreaftingStationFactory
    {
        internal static GameObject Create(WorkbenchData workbenchData)
        {
            GameObject workbench = new GameObject("workbench");
            workbench.AddComponent(new PacketDistributorComponent());
            workbench.AddComponent(new UnitNicknameComponent("workbench"));
            AddTransform(workbench, workbenchData);
            workbench.AddComponent(new WorkbenchViewComponent(workbenchData.WorkbenchType));
            workbench.AddComponent(new AnimatorComponent());
            workbench.AddComponent(new EntityTagComponent());
            workbench.AddComponent(new PlayersNetworkTransmissionComponent());


            //Interaction
            workbench.AddComponent(new InteractiveObjectTagComponent());
            workbench.AddComponent(new WorkbenchInteractionComponent());
            workbench.AddComponent(new WorkbenchInteractionControllerComponent());
            return workbench;
        }

        private static void AddTransform(GameObject workbench, WorkbenchData workbenchData)
        {
            var transform = new TransformComponent();
            //transform.UpdatePosition(workbenchData.Position);
            //transform.UpdateRotation(workbenchData.Rotation);
            workbench.AddComponent(transform);
        }
    }
}
