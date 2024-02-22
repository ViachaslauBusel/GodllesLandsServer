using Game.NetworkTransmission;
using Game.Physics;
using NetworkGameEngine;
using Zenject;

namespace Game.GridMap.Scripts
{
    /// <summary>
    /// Указывает что обьект содержит данные которые необходимо синхронизировать
    /// </summary>
    internal class PlayerEntityTagComponent : EntityTagComponent
    {
        protected override Entity CreateEntity()
        {
            var transmission = GetComponent<NetworkTransmissionComponent>();
            return new PlayerEntity(GameObject, transmission.Socket);
        }
    }
}
