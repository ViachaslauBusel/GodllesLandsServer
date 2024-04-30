using Game.Drop;
using Game.Equipment.Components;
using Game.Inventory.Components;
using Game.Items;
using Game.Items.Components;
using Game.Messenger;
using Game.NetworkTransmission;
using Game.Physics.Transform;
using Godless_Lands_Game.GameObjectFactory;
using NetworkGameEngine;
using Protocol;
using Protocol.Data.Replicated.Skins;
using Protocol.MSG.Game.Drop;
using Protocol.MSG.Game.Messenger;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Items.Components
{
    public class ItemListenerComponent : Component
    {
        private InventoryComponent _inventory;
        private EquipmentComponent _equipment;
        private ItemStorageComponent _itemStorage;
        private NetworkTransmissionComponent _networkTransmission;
        private TransformComponent _transform;
        private MessageBroadcastComponent _messageBroadcast;

        public override void Init()
        {
            _inventory = GetComponent<InventoryComponent>();
            _equipment = GetComponent<EquipmentComponent>();
            _itemStorage = GetComponent<ItemStorageComponent>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _transform = GetComponent<TransformComponent>();
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_DROP_ITEM, DropItemOnGround);
        }

        private async void DropItemOnGround(Packet packet)
        {
            packet.Read(out MSG_DROP_ITEM_CS msg);

            if(Vector3.Distance(_transform.Position, msg.Position) > 10f)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "To far from drop position");
                return;
            }

            Item item = _inventory.TakeItem(msg.ItemUID, msg.Count);
            item ??= _equipment.TakeItem(msg.ItemUID);

            if ( item != null)
            {
                await _itemStorage.DestroyItem(item);

                var dropHolder = new DropHolderComponent();
                dropHolder.AddItem(item);
                GameObject dropBag = DropBagFactory.CreateDropBag(msg.Position, DropBagType.Medium, dropHolder);
                _ = GameObject.World.AddGameObject(dropBag);
            }
            else _messageBroadcast.SendMessage(MsgLayer.System, "Item not found");
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_DROP_ITEM);
        }
    }
}
