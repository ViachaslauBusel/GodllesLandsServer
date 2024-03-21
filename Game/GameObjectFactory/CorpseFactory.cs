﻿using Game.AI;
using Game.Animation;
using Game.Corpse;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.Monsters.Components;
using Game.NetworkTransmission;
using Game.Pathfinding;
using Game.Physics.DynamicObjects;
using Game.Physics.Transform;
using Game.Skills;
using Game.Systems.Stats;
using Game.Systems.Target;
using Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Monsters;
using Protocol.Data.Replicated.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.GameObjectFactory
{
    public static class CorpseFactory
    {
        public static GameObject CreateCorpse(Vector3 position, IViewComponent viewComponent, DropHolderComponent dropHolder)
        {
            GameObject corpse = new GameObject("corpse");
            corpse.AddComponent(new PacketDistributorComponent());
            AddTransformComponent(corpse, position);
            AddViewComponent(viewComponent, corpse);
            corpse.AddComponent(new DynamicObjectComponent());
            corpse.AddComponent(new EntityTagComponent());
            corpse.AddComponent(new AnimatorComponent());
            corpse.AddComponent(dropHolder);
            corpse.AddComponent(new CorpseControllerComponent());
            corpse.AddComponent(new DropInteractionComponent());
            corpse.AddComponent(new DropListenerComponent());
            corpse.AddComponent(new PlayersNetworkTransmissionComponent());
            corpse.AddComponent(new DeadObjectCleanerComponent());
            return corpse;
        }

        private static void AddViewComponent(IViewComponent viewComponent, GameObject corpse)
        {
            viewComponent.SetNeedChaceVisual(false);
            corpse.AddComponent(viewComponent as Component);
        }

        private static void AddTransformComponent(GameObject corpse, Vector3 position)
        {
            var transform = new TransformComponent();
            transform.UpdatePosition(position);
            corpse.AddComponent(transform);
        }
    }
}
