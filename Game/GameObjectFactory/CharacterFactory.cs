using Game.Animation;
using Game.CombatModeControl.Components;
using Game.DB;
using Game.Equipment.Components;
using Game.GridMap.Scripts;
using Game.Hotbar;
using Game.Inventory.Components;
using Game.Items.Components;
using Game.Messenger;
using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Game.Physics.DynamicObjects;
using Game.Physics.PlayerInput.Components;
using Game.Physics.PlayerInput.Scripts;
using Game.Physics.Transform;
using Game.PlayerScene;
using Game.Replication.Scripts;
using Game.Skills;
using Game.Systems.Stats.Components;
using Game.Systems.Target;
using Game.Systems.TargetSystem;
using Game.Tests;
using Game.UnitVisualization;
using Godless_Lands_Game.Items.Components;
using Godless_Lands_Game.Professions.Components;
using Godless_Lands_Game.Quests.Components;
using Godless_Lands_Game.Systems.Stats.Components;
using NetworkGameEngine;
using NetworkGameEngine.Units.Characters;

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
            character.AddComponent(new TransformDbSyncComponent());
            character.AddComponent(new CharacterViewComponent());
            character.AddComponent(new DynamicObjectComponent());
            character.AddComponent(new PlayerEntityTagComponent());
            character.AddComponent(new ReplicationTagComponent());
            character.AddComponent(new PlayerSceneControllerComponent());
     
            character.AddComponent(new HotbarComponent());
            character.AddComponent(new AnimatorComponent());
            character.AddComponent(new ObjectInteractionProcessorComponent());
            character.AddComponent(new RaycastTest());
            //Movement
            character.AddComponent(new PlayerInputComponent());
            character.AddComponent(new MovementSpeedValidatorComponent());
            //Messenger
            character.AddComponent(new MessageReceiverComponent());
            character.AddComponent(new MessageBroadcastComponent());
            //Target
            character.AddComponent(new TargetListenerComponent());
            character.AddComponent(new TargetManagerComponent());
            character.AddComponent(new TargetedUnitTrackerComponent());
            character.AddComponent(new ClientSyncTargetComponent());
            character.AddComponent(new TargetCommandHandlerComponent());
            //Stats
            character.AddComponent(new CharacterStatsComponent());
            character.AddComponent(new BodyComponent());
            character.AddComponent(new PlayerDeathStateComponent());
            character.AddComponent(new CharacterStatsCalculatorComponent());
            character.AddComponent(new BodyRegenerationComponent());
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
            character.AddComponent(new OfflineItemAdderComponent());
            //Equipment
            character.AddComponent(new EquipmentComponent());
            character.AddComponent(new EquipmentClientSyncComponent());
            character.AddComponent(new EquipmentDbSyncComponent());
            character.AddComponent(new EquipmentListenerComponent());

            //Combat mode
            character.AddComponent(new CombatModeComponent());
            character.AddComponent(new CombatModeListenerComponent());

            //Professions
            character.AddComponent(new ProfessionsComponent());
            character.AddComponent(new ProfessionsClientSyncComponent());
            character.AddComponent(new ProfessionsDbSyncComponent());
            character.AddComponent(new ProfessionsCommandHandlerComponent());

            //Quests
            character.AddComponent(new QuestControllerComponent());
            character.AddComponent(new QuestClientSyncComponent());
            character.AddComponent(new QuestDbSyncComponent());
            character.AddComponent(new QuestNodeHandlerStorageComponent());
            character.AddComponent(new QuestsListenerComponent());
            return character;
        }
    }
}
