using Game.Inventory.Components;
using Game.Items;
using Game.Messenger;
using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Godless_Lands_Game.Inventory;
using Godless_Lands_Game.Recipes;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol;
using Protocol.Data.Recipes;
using Protocol.Data.Workbenches;
using Protocol.MSG.Game.Messenger;
using Protocol.MSG.Game.Workbench;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Godless_Lands_Game.ObjectInteraction.Workbench
{
    public class CharacterCraftControllerComponent : Component, ICharacterWorkbenchController
    {
        private volatile bool _isReadyForWork = false;

        private NetworkTransmissionComponent _networkTransmission;
        private RecipesDataStorageService _recipesDataStorage;
        private ItemsFactory _itemsFactory;
        private InventoryComponent _inventory;
        private MessageBroadcastComponent _messageBroadcast;
        private ObjectInteractionProcessorComponent _objectInteractionProcessor;
        private WorkbenchType _workbenchInitiator;

        public bool IsReadyForWork => _isReadyForWork;

        public CharacterCraftControllerComponent(WorkbenchType workbenchType)
        {
            _workbenchInitiator = workbenchType;
        }

        [Inject]
        private void Construct(RecipesDataStorageService recipesDataStorage, ItemsFactory itemsFactory)
        {
            _recipesDataStorage = recipesDataStorage;
            _itemsFactory = itemsFactory;
        }

        public override void Init()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _inventory = GetComponent<InventoryComponent>();
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();
            _objectInteractionProcessor = GetComponent<ObjectInteractionProcessorComponent>();

            _networkTransmission.RegisterHandler(Opcode.MSG_WORKBENCH_CREATE_ITEM, CreateItem);

            _isReadyForWork = true;
        }

        private void CreateItem(Packet packet)
        {
            packet.Read(out MSG_WORKBENCH_CREATE_ITEM request);

            RecipeData recipe = _recipesDataStorage.GetRecipe(request.RecipeID);

            if (recipe == null)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Recipe not found");
                return;
            }

            if (_workbenchInitiator != recipe.workbench)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Wrong workbench type");
                return;
            }

             List<ItemSlot> takedItems = new List<ItemSlot>();

            foreach(RecipeComponent component in recipe.Components)
            {
                List<ItemSlot> items = _inventory.TakeItemByItemId(component.ItemID, component.Amount);
                if (items == null)
                {
                    ReturnItemsToInventory(takedItems);
                    _messageBroadcast.SendMessage(MsgLayer.System, "Not enough items");
                    return;
                }
                takedItems.AddRange(items);
            }

            foreach(var itemToRemove in takedItems)
            {
                _inventory.RemoveItem(itemToRemove.Item.UniqueID, itemToRemove.Item.Count);
            }

            Item resultItem = _itemsFactory.CreateItem(recipe.ResultItemID, 0, count: 1);
            _inventory.AddItem(resultItem);
            _messageBroadcast.SendMessage(MsgLayer.System, "Item created");
        }

        private void ReturnItemsToInventory(List<ItemSlot> takedItems)
        {
            foreach (var item in takedItems)
            {

                if(_inventory.AddItem(item.BagType, item.SlotIndex, item.Item) == false)
                    Debug.Log.Fatal($"Failed to return item:{item.Item.UniqueID} to inventory");
                else Debug.Log.Info($"Items:{item.Item.UniqueID} returned to inventory");
            }
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_WORKBENCH_CREATE_ITEM);
        }
    }
}
