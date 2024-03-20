using Game.AI;
using Game.Animation;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.Monsters.Components;
using Game.NetworkTransmission;
using Game.Pathfinding;
using Game.Physics.DynamicObjects;
using Game.Physics.Transform;
using Game.Skills;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Monsters;

namespace Game.GameObjectFactory
{
    public static class MonsterFactory
    {
        public static GameObject CreateMonster(MonsterData data)
        {
            GameObject monster = new GameObject("monster");
            monster.AddComponent(new PacketDistributorComponent());
            monster.AddComponent(new MobStatComponent(data.Name));
            monster.AddComponent(new BodyComponent());
            monster.AddComponent(new TransformComponent());
            monster.AddComponent(new SpawnComponent(data.SpawnPosition, data.SpawnRadius));
            monster.AddComponent(new MonsterViewComponent(data.SkinID));
            monster.AddComponent(new DynamicObjectComponent());
            monster.AddComponent(new EntityTagComponent());
            monster.AddComponent(new AnimatorComponent());
            monster.AddComponent(new AiControllerComponent());
            monster.AddComponent(new UnitPathMoverComponent());
            monster.AddComponent(new TargetManagerComponent());
            monster.AddComponent(new SkillsStoreComponent());
            monster.AddComponent(new SkillUsageComponent());
            monster.AddComponent(new LootDropOnDeathComponent());
            return monster;
        }
    }
}
