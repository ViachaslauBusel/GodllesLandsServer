using Game.AI;
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
            AddPacketDistributorComponent(corpse);
            AddTransformComponent(corpse, position);
            AddViewComponent(corpse, viewComponent);
            AddDynamicObjectComponent(corpse);
            AddEntityTagComponent(corpse);
            AddAnimatorComponent(corpse);
            AddDropHolderComponent(corpse, dropHolder);
            corpse.AddComponent(new CorpseControllerComponent());
            return corpse;
        }

        private static void AddPacketDistributorComponent(GameObject corpse)
        {
            corpse.AddComponent(new PacketDistributorComponent());
        }

        private static void AddTransformComponent(GameObject corpse, Vector3 position)
        {
            var transform = new TransformComponent();
            transform.UpdatePosition(position);
            corpse.AddComponent(transform);
        }

        private static void AddViewComponent(GameObject corpse, IViewComponent viewComponent)
        {
            corpse.AddComponent(viewComponent as Component);
        }

        private static void AddDynamicObjectComponent(GameObject corpse)
        {
            corpse.AddComponent(new DynamicObjectComponent());
        }

        private static void AddEntityTagComponent(GameObject corpse)
        {
            corpse.AddComponent(new EntityTagComponent());
        }

        private static void AddAnimatorComponent(GameObject corpse)
        {
            corpse.AddComponent(new AnimatorComponent());
        }

        private static void AddDropHolderComponent(GameObject corpse, DropHolderComponent dropHolder)
        {
            corpse.AddComponent(dropHolder);
        }
    }
}
