using Game.Animation;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Game.Physics.DynamicObjects;
using Game.Physics.Transform;
using Game.Units.Corpse;
using Game.UnitVisualization;
using Godless_Lands_Game.Drop;
using NetworkGameEngine;
using System.Numerics;

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
            corpse.AddComponent(new InteractiveObjectTagComponent());
            corpse.AddComponent(new DropInteractionComponent());
            corpse.AddComponent(new DropClientSyncComponent());
            corpse.AddComponent(new DropListenerComponent());
            corpse.AddComponent(new PlayersNetworkTransmissionComponent());
            corpse.AddComponent(new TimedDestructionComponent());
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
