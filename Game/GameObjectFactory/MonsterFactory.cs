using Game.AI;
using Game.Animation;
using Game.GridMap.Scripts;
using Game.Monsters.Components;
using Game.NetworkTransmission;
using Game.Pathfinding;
using Game.Physics;
using Game.Physics.DynamicObjects;
using Game.Physics.Transform;
using Game.Systems.Stats;
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
            monster.AddComponent(new DamageReceiverComponent());
            monster.AddComponent(new TransformComponent());
            monster.AddComponent(new SpawnComponent(data.SpawnPosition, data.SpawnRadius));
            monster.AddComponent(new MonsterViewComponent(data.SkinID));
            monster.AddComponent(new DynamicObjectComponent());
            monster.AddComponent(new EntityTagComponent());
            monster.AddComponent(new AnimatorComponent());
            monster.AddComponent(new AiControllerComponent());
            monster.AddComponent(new UnitPathMoverComponent());
            return monster;
        }
    }
}
