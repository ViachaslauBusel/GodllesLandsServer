using Game.Animation;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.ObjectInteraction.MiningStone;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Units;
using Game.Units.Resources;
using Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Resources;
using Protocol.Data.SpawnData;

namespace Game.GameObjectFactory
{
    internal class ResourceStoneFactory
    {
        internal static GameObject Create(ResourceInfo miningStone, SpawnUnitPointData spawn)
        {
            GameObject stone = new GameObject("miningStone");
            stone.AddComponent(new PacketDistributorComponent());
            stone.AddComponent(new UnitNicknameComponent("stone"));
            stone.AddComponent(new StatsComponent());
            stone.AddComponent(new BodyComponent());
            stone.AddComponent(new TransformComponent());
            stone.AddComponent(new RespawnComponent(spawn));
            stone.AddComponent(new MiningStoneViewComponent(miningStone.ID));
            stone.AddComponent(new AnimatorComponent());
            stone.AddComponent(new ResourceVisibilityToggleComponent());


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
