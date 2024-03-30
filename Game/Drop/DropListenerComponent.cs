using Game.Inventory.Commands;
using Game.Items;
using Game.NetworkTransmission;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.MSG.Game.Drop;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Drop
{

    /// <summary>
    /// This component is responsible for listening network command from the client
    /// </summary>
    public class DropListenerComponent : Component
    {
        private DropInteractionComponent _dropInteraction;
        private DropHolderComponent _dropHolder;
        private PlayersNetworkTransmissionComponent _playersNetworkTransmission;

        public override void Init()
        {
            _dropInteraction = GetComponent<DropInteractionComponent>();
            _dropHolder = GetComponent<DropHolderComponent>();
            _playersNetworkTransmission = GetComponent<PlayersNetworkTransmissionComponent>();

            _dropInteraction.OnClientConnected += OnClientConnected;
            _dropInteraction.OnClientDisconnected += OnClientDisconnected;
     
        }

        private void OnClientDisconnected(GameObject charObj, IPlayerNetworkProfile playerProfile)
        {
            Debug.Log.Info($"Player {playerProfile.CharacterObjectID} disconnected from drop interaction >>>");
            _playersNetworkTransmission.UnregisterHandler(playerProfile, Opcode.MSG_TAKE_DROP);
        }

        private void OnClientConnected(GameObject charObj, IPlayerNetworkProfile playerProfile, List<Component> components)
        {
            Debug.Log.Info($"Player {playerProfile.CharacterObjectID} connected to drop interaction <<<");
            _playersNetworkTransmission.RegisterHandler(playerProfile, Opcode.MSG_TAKE_DROP, OnTakeDrop);
        }

        private void OnTakeDrop(IPlayerNetworkProfile playerProfile, Packet packet)
        {
            packet.Read(out MSG_DROP_COMMAND_CS request);

            Debug.Log.Info($"Received command:{request.CommandType} from {playerProfile.CharacterObjectID}");

            switch (request.CommandType)
            {
                case DropCommandType.TakeItem:
                    TakeItemAsync(playerProfile, request.DropIndex);
                    break;
                case DropCommandType.TakeAllItems:
                    TakeAllDrop(playerProfile);
                    break;
                case DropCommandType.EndInteraction:
                    _dropInteraction.DisconnectClient(playerProfile.CharacterObjectID);
                    break;
            }
        }

        private void TakeItemAsync(IPlayerNetworkProfile playerProfile, int dropIndex)
        {
            Item item = _dropHolder.TakeItem(dropIndex);
            if (item == null)
            {
                Debug.Log.Warn($"[TakeItem] Item with index {dropIndex} not found");
                return;
            }

            List<Item> items = new List<Item> { item };
            TransferItemsToInventory(items, playerProfile.CharacterObjectID);
        }

        private void TakeAllDrop(IPlayerNetworkProfile playerProfile)
        {
            _dropInteraction.DisconnectClient(playerProfile.CharacterObjectID);

            var allDrop = _dropHolder.TakeAll();

            TransferItemsToInventory(allDrop, playerProfile.CharacterObjectID);
        }

        private async void TransferItemsToInventory(List<Item> items, int characterObjectId)
        {
            if (GameObject.World.TryGetGameObject(characterObjectId, out var character) == false)
            {
                Debug.Log.Error($"[TakeAllDrop] Character with id {characterObjectId} not found");
                return;
            }

            AddItemToInventoryCommand command = new AddItemToInventoryCommand
            {
                Items = items
            };

            //Return list item that can't be added to inventory
            var job = character.SendCommandAndReturnResult<AddItemToInventoryCommand, List<Item>>(command, 1_000);

            await job;

            if (job.IsCompleted == false)
            {
                Debug.Log.Fatal($"[TakeAllDrop] Error while adding items to inventory");
                return;
            }

            if (job.IsFaulted)
            {
                Debug.Log.Warn($"[TakeAllDrop] Can't add items to inventory");
                //Return items to drop
                _dropHolder.AddItems(items);
                return;
            }

            items = job.GetResult();

            if (items.Count > 0)
            {
                Debug.Log.Warn($"[TakeAllDrop][RETURN] Can't add items to inventory");
                //Return items to drop
                _dropHolder.AddItems(items);
            }
        }

        public override void OnDestroy()
        {
            _dropInteraction.OnClientConnected -= OnClientConnected;
            _dropInteraction.OnClientDisconnected -= OnClientDisconnected;
        }
    }
}
