using Game.Inventory;
using Game.Inventory.Components;
using Game.Items;
using Game.Items.Components;
using Game.Messenger;
using Game.NetworkTransmission;
using Game.ObjectInteraction;
using Godless_Lands_Game.Recipes;
using NetworkGameEngine;
using Protocol;
using Protocol.Data.Recipes;
using Protocol.Data.Workbenches;
using Protocol.MSG.Game.Inventory;
using Protocol.MSG.Game.Messenger;
using Protocol.MSG.Game.Workbench;
using RUCP;
using Zenject;

namespace Godless_Lands_Game.ObjectInteraction.Workbench
{
    internal class CharacterSmelterControllerComponent : Component, ICharacterWorkbenchController
    {
        class ItemAmount
        {
            public long ItemUID { get; set; }
            public int ItemID { get; set; }
            public int Amount { get; set; }
        }

        private volatile bool _isReadyForWork = false;

        private NetworkTransmissionComponent _networkTransmission;
        private RecipesDataStorageService _recipesDataStorage;
        private ItemsFactory _itemsFactory;
        private InventoryComponent _inventory;
        private MessageBroadcastComponent _messageBroadcast;
        private ObjectInteractionProcessorComponent _objectInteractionProcessor;
        private WorkbenchType _workbenchInitiator;

        public bool IsReadyForWork => _isReadyForWork;

        public CharacterSmelterControllerComponent(WorkbenchType workbenchType)
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

            if(_workbenchInitiator != recipe.workbench)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Wrong workbench type");
                return;
            }

            // List of components that client put in the workbench. There can be more items that required by the recipe
            List<ItemAmount> components = GetIndegrientsFromInventory(request.Components);
            List<ItemAmount> fuel = GetIndegrientsFromInventory(request.Fuel);

            List<ItemAmount> itemsToRemove = new List<ItemAmount>();


            ProcessIngredients(components, recipe.Components, itemsToRemove);
            ProcessIngredients(fuel, recipe.Fuel, itemsToRemove);

            foreach (ItemAmount toRemove in itemsToRemove)
            {
                if(_inventory.RemoveItem(toRemove.ItemUID, toRemove.Amount) == false)
                {
                    _messageBroadcast.SendMessage(MsgLayer.System, "Failed to remove item");
                    return;
                }
            }

            Item resultItem = _itemsFactory.CreateItem(recipe.ResultItemID, 0, count: 1);
            _inventory.AddItem(resultItem);
            _messageBroadcast.SendMessage(MsgLayer.System, "Item created");
        }

        /// <summary>
        /// Swap ingredients from the inventory to the list of items to remove
        /// </summary>
        /// <param name="ingredients"></param>
        /// <param name="recipeIngredients"></param>
        /// <param name="itemsToRemove"></param>
        private void ProcessIngredients(List<ItemAmount> ingredients, List<RecipeComponent> recipeIngredients, List<ItemAmount> itemsToRemove)
        {
            foreach (var ingredient in recipeIngredients)
            {
                if (ingredient == null)
                {
                    _messageBroadcast.SendMessage(MsgLayer.System, "Component not found");
                    return;
                }

                if (!TakeFromIngredient(ingredients, ingredient.ItemID, ingredient.Amount, out ItemAmount itemAmount))
                {
                    _messageBroadcast.SendMessage(MsgLayer.System, "Not enough ingredients");
                    return;
                }

                itemsToRemove.Add(itemAmount);
            }
        }

        private bool TakeFromIngredient(List<ItemAmount> components, int itemID, int amount, out ItemAmount itemAmount)
        {
            for (int i = 0; i < components.Count; i++) 
            {
                if (components[i].ItemID == itemID && components[i].Amount >= amount)
                {
                    itemAmount = new ItemAmount { ItemUID = components[i].ItemUID, ItemID = components[i].ItemID, Amount = amount };
                    components[i].Amount -= amount;
                    return true;
                }
            }
            itemAmount = null;
            return false;
        }

        private List<ItemAmount> GetIndegrientsFromInventory(List<long> components)
        {
            List<ItemAmount> items = new List<ItemAmount>();
            foreach (long componentID in components.Distinct()) 
            {
                Item item = _inventory.GetItem(componentID);
                if (item == null)
                {
                    _messageBroadcast.SendMessage(MsgLayer.System, "Component not found");
                    continue;
                }
                items.Add(new ItemAmount()
                {
                    ItemID = item.Data.ID,
                    ItemUID = item.UniqueID,
                    Amount = item.Count
                });
            }
            return items;
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_WORKBENCH_CREATE_ITEM);
        }
    }
}
