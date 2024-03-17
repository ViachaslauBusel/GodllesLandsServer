﻿using Game.Animation;
using Game.DB;
using Game.GridMap.Scripts;
using Game.Hotbar;
using Game.Messenger;
using Game.NetworkTransmission;
using Game.Physics;
using Game.Physics.DynamicObjects;
using Game.Physics.PlayerInput.Scripts;
using Game.Physics.Transform;
using Game.PlayerScene;
using Game.Replication.Scripts;
using Game.Skills;
using Game.UnitVisualization;
using Game.Systems.Stats;
using Game.Systems.TargetSystem;
using Game.Tests;
using NetworkGameEngine;
using NetworkGameEngine.Units.Characters;
using RUCP;
using RUCP.Handler;
using Game.ObjectInteraction;
using Game.Inventory.Components;
using Game.Items.Components;

namespace Game.GameObjectFactory
{
    internal static class CharacterFactory
    {
        public static GameObject Create(int characterID, IPlayerNetworkProfile playerProfile)
        {
            GameObject character = new GameObject("chacter");
            character.AddComponent(new NetworkTransmissionComponent(playerProfile));
            character.AddComponent(new DBControlComponent());
            character.AddComponent(new PacketDistributorComponent());
            character.AddComponent(new CharacterInfoHolder(characterID));
            character.AddComponent(new CharacterStatsComponent());
            character.AddComponent(new BodyComponent());
            character.AddComponent(new PlayerDeathStateComponent());
            character.AddComponent(new PlayerTransformComponent());
            character.AddComponent(new CharacterViewComponent());
            character.AddComponent(new DynamicObjectComponent());
            character.AddComponent(new PlayerInputComponent());
            character.AddComponent(new PlayerEntityTagComponent());
            character.AddComponent(new ReplicationTagComponent());
            character.AddComponent(new PlayerSceneStatusComponent());
            character.AddComponent(new CharacterTargetManagerComponent());
            character.AddComponent(new CharacterSkillsStoreComponent());
            character.AddComponent(new HotbarComponent());
            character.AddComponent(new CharacterSkillUsageComponent());
            character.AddComponent(new MessageReceiverComponent());
            character.AddComponent(new AnimatorComponent());
            character.AddComponent(new ObjectInteractionProcessorComponent());
            character.AddComponent(new RaycastTest());
            //Inventory
            character.AddComponent(new InventoryComponent());
            character.AddComponent(new InventoryClientSyncComponent());
            character.AddComponent(new InventoryCommandHandlerComponent());
            character.AddComponent(new InventoryDbSyncComponent());
            character.AddComponent(new InventoryListenerComponent());
            character.AddComponent(new ItemStorageComponent());
            character.AddComponent(new ItemUsageComponent());
            return character;
        }
    }
}
