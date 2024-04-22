using Game.Animation;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Game.Physics.Transform;
using Game.Units.Corpse;
using Godless_Lands_Game.Drop;
using Godless_Lands_Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using System.Numerics;

namespace Godless_Lands_Game.GameObjectFactory
{
    internal static class DropBagFactory
    {
        public static GameObject CreateDropBag(Vector3 position, DropBagType bagType, DropHolderComponent dropHolder)
        {
            GameObject corpse = new GameObject("DropBag");
            corpse.AddComponent(new PacketDistributorComponent());
            AddTransformComponent(corpse, position, 0f);
            corpse.AddComponent(new DropBagViewComponent(bagType));
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

        private static void AddTransformComponent(GameObject corpse, Vector3 position, float rotation)
        {
            var transform = new TransformComponent();
            transform.UpdatePosition(position);
            transform.UpdateRotation(rotation);
            corpse.AddComponent(transform);
        }
    }
}
