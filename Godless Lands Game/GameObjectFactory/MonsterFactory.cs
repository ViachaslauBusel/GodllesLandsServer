﻿using Game.AI;
using Game.Animation;
using Game.Drop;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.Pathfinding;
using Game.Physics.DynamicObjects;
using Game.Physics.Transform;
using Game.Skills;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Units.Monsters.Components;
using Game.UnitVisualization;
using Godless_Lands_Game.Systems.Stats.Components;
using NetworkGameEngine;
using Protocol.Data.Monsters;
using Protocol.Data.SpawnData;

namespace Game.GameObjectFactory
{
    public static class MonsterFactory
    {
        public static GameObject CreateMonster(MonsterInfo data, SpawnUnitPointData spawn)
        {
            GameObject monster = new GameObject("monster");
            monster.AddComponent(new PacketDistributorComponent());
            monster.AddComponent(new StatsComponent());
            monster.AddComponent(new BodyComponent());
            monster.AddComponent(new TransformComponent());
            monster.AddComponent(new SpawnComponent(spawn));
            monster.AddComponent(new MonsterViewComponent(data.ID));
            monster.AddComponent(new DynamicObjectComponent());
            monster.AddComponent(new EntityTagComponent());
            monster.AddComponent(new AnimatorComponent());
            monster.AddComponent(new AiControllerComponent());
            monster.AddComponent(new UnitPathMoverComponent());
            monster.AddComponent(new SkillsStoreComponent());
            monster.AddComponent(new SkillUsageComponent());
            monster.AddComponent(new LootDropOnDeathComponent());

            //Stats
            monster.AddComponent(new TargetManagerComponent());
            monster.AddComponent(new TargetedUnitTrackerComponent());
            monster.AddComponent(new BodyRegenerationComponent());
            return monster;
        }
    }
}
