using Game.Animation;
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
using Game.Equipment.Components;
using Game.Systems.Stats.Components;
using Game.Systems.Target;

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
            character.AddComponent(new PlayerTransformComponent());
            character.AddComponent(new CharacterViewComponent());
            character.AddComponent(new DynamicObjectComponent());
            character.AddComponent(new PlayerInputComponent());
            character.AddComponent(new PlayerEntityTagComponent());
            character.AddComponent(new ReplicationTagComponent());
            character.AddComponent(new PlayerSceneStatusComponent());
     
            character.AddComponent(new HotbarComponent());
            character.AddComponent(new MessageReceiverComponent());
            character.AddComponent(new AnimatorComponent());
            character.AddComponent(new ObjectInteractionProcessorComponent());
            character.AddComponent(new RaycastTest());
            //Target
            character.AddComponent(new TargetListenerComponent());
            character.AddComponent(new TargetManagerComponent());
            character.AddComponent(new TargetedUnitTrackerComponent());
            character.AddComponent(new ClientSyncTargetComponent());
            //Stats
            character.AddComponent(new CharacterStatsComponent());
            character.AddComponent(new BodyComponent());
            character.AddComponent(new PlayerDeathStateComponent());
            character.AddComponent(new CharacterStatsCalculatorComponent());
            //Skills
            character.AddComponent(new CharacterSkillsStoreComponent());
            character.AddComponent(new CharacterSkillUsageComponent());
            //Inventory
            character.AddComponent(new InventoryComponent());
            character.AddComponent(new InventoryClientSyncComponent());
            character.AddComponent(new InventoryDbSyncComponent());
            character.AddComponent(new InventoryCommandHandlerComponent());
            character.AddComponent(new InventoryListenerComponent());
            //Items
            character.AddComponent(new ItemStorageComponent());
            character.AddComponent(new ItemUsageComponent());
            //Equipment
            character.AddComponent(new EquipmentComponent());
            character.AddComponent(new EquipmentClientSyncComponent());
            character.AddComponent(new EquipmentDbSyncComponent());
            character.AddComponent(new EquipmentListenerComponent());
            return character;
        }
    }
}
