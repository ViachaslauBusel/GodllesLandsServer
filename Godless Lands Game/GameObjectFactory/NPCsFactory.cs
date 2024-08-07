﻿using Game.Animation;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.Physics.Transform;
using Game.Units.Monsters.Components;
using Godless_Lands_Game.Units.NPCs.Components;
using Godless_Lands_Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.NPCs;
using Protocol.Data.SpawnData;

namespace Godless_Lands_Game.GameObjectFactory
{
    internal class NPCsFactory
    {
        public static GameObject CreateNPC(NpcInfo data, SpawnUnitPointData spawn)
        {
            GameObject monster = new GameObject("NPC");
            monster.AddComponent(new PacketDistributorComponent());
            monster.AddComponent(new TransformComponent());
            monster.AddComponent(new SpawnComponent(spawn));
            monster.AddComponent(new NpcViewComponent(data.ID));
            monster.AddComponent(new EntityTagComponent());
            monster.AddComponent(new AnimatorComponent());

            //Stats
            return monster;
        }
    }
}
