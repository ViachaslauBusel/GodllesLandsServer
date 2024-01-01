using Game.GridMap.Scripts;
using Game.NetworkTransmission;
using Game.Physics;
using Game.Physics.DynamicObjects;
using Game.Physics.PlayerInput.Scripts;
using Game.PlayerScene;
using Game.Replication.Scripts;
using Game.Skins;
using Game.Systems.Stats;
using Game.Systems.TargetSystem;
using NetworkGameEngine;
using NetworkGameEngine.Units.Characters;
using RUCP;
using RUCP.Handler;

namespace Game.GameObjectFactory
{
    internal static class CharacterFactory
    {
        public static GameObject Create(int characterID, Client socket, HandlersStorage<Action<Profile, Packet>> handlersStorage)
        {
            GameObject character = new GameObject();
            character.AddComponent(new NetworkTransmissionComponent(socket, handlersStorage));
            character.AddComponent(new CharacterInfoHolder(characterID));
            character.AddComponent(new CharacterStatsComponent());
            character.AddComponent(new TransformComponent());
            character.AddComponent(new CharacterViewComponent());
            character.AddComponent(new DynamicObjectComponent());
            character.AddComponent(new PlayerInputComponent());
            character.AddComponent(new MapObjectTagComponent());
            character.AddComponent(new ReplicationTagComponent());
            character.AddComponent(new PlayerSceneStatusComponent());
            character.AddComponent(new UnitTargetSelectionComponent());
            return character;
        }
    }
}
