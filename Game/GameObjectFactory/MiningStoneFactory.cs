using Game.Animation;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.ObjectInteraction.MiningStone;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Units;
using Game.Units.MiningStones;
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
            stone.AddComponent(new UnitNicknameComponent("stone"));
            stone.AddComponent(new StatsComponent());
            stone.AddComponent(new BodyComponent());
            stone.AddComponent(new TransformComponent());
            stone.AddComponent(new RespawnComponent(miningStone.SpawnPoint, miningStone.SpawnRadius, miningStone.StartSpawnTime, miningStone.TimeSpawn));
            stone.AddComponent(new MiningStoneViewComponent(miningStone.ID));
            stone.AddComponent(new AnimatorComponent());
            stone.AddComponent(new MiningStoneVisibilityToggleComponent());


            //Drop
            stone.AddComponent(new LootDropOnReviveComponent(miningStone.Drops));
            stone.AddComponent(new DropHolderComponent());

            //Stats
            stone.AddComponent(new TargetManagerComponent());
            stone.AddComponent(new TargetedUnitTrackerComponent());

            //Interaction
            stone.AddComponent(new MiningStoneInteractionComponent());
            stone.AddComponent(new MiningStoneCommandHandlerComponent());
            return stone;
        }
    }
}
