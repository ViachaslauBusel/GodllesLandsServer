using Game.Animation;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.Physics.DynamicObjects;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Units.Monsters.Components;
using Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.MiningStone;

namespace Game.GameObjectFactory
{
    internal class MiningStoneFactory
    {
        internal static GameObject CreateStone(ResourceMiningStoneData miningStone)
        {
            GameObject stone = new GameObject("miningStone");
            stone.AddComponent(new PacketDistributorComponent());
            stone.AddComponent(new MobStatComponent("stone"));
            stone.AddComponent(new BodyComponent());
            stone.AddComponent(new TransformComponent());
            stone.AddComponent(new SpawnComponent(miningStone.SpawnPoint, miningStone.SpawnRadius));
            stone.AddComponent(new MiningStoneViewComponent(miningStone.ID));
            stone.AddComponent(new DynamicObjectComponent());
            stone.AddComponent(new EntityTagComponent());
            stone.AddComponent(new AnimatorComponent());
            stone.AddComponent(new LootDropOnDeathComponent());

            //Stats
            stone.AddComponent(new TargetManagerComponent());
            stone.AddComponent(new TargetedUnitTrackerComponent());
            return stone;
        }
    }
}
