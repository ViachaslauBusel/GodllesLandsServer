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
using Godless_Lands_Game.UnitVisualization;
using NetworkGameEngine;
using System.Numerics;

namespace Game.GameObjectFactory
{
    public static class CorpseFactory
    {
        public static GameObject CreateCorpse(Vector3 position, float rotation, int skinId, int cachedObjectId, DropHolderComponent dropHolder)
        {
            GameObject corpse = new GameObject("corpse");
            corpse.AddComponent(new PacketDistributorComponent());
            AddTransformComponent(corpse, position, rotation);
            corpse.AddComponent(new CorpseViewComponent(skinId, cachedObjectId));
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
