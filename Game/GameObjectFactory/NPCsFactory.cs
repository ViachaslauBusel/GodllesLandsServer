using Game.Animation;
using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.Physics.Transform;
using Godless_Lands_Game.Units.NPCs.Components;
using Godless_Lands_Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.NPCs;

namespace Godless_Lands_Game.GameObjectFactory
{
    internal class NPCsFactory
    {
        public static GameObject CreateNPC(NpcData data)
        {
            GameObject monster = new GameObject("NPC");
            monster.AddComponent(new PacketDistributorComponent());
            monster.AddComponent(new TransformComponent());
            monster.AddComponent(new NpcSpawnComponent(data.spawnPoint, data.spawnRotation));
            monster.AddComponent(new NpcViewComponent(data.id));
            monster.AddComponent(new EntityTagComponent());
            monster.AddComponent(new AnimatorComponent());

            //Stats
            return monster;
        }
    }
}
