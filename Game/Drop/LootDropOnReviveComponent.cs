using Game.Items;
using Game.Systems.Stats.Components;
using Game.Tools;
using NetworkGameEngine;
using Protocol.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.Drop
{
    public class LootDropOnReviveComponent : Component
    {
        private BodyComponent _bodyComponent;
        private DropHolderComponent _dropHolder;
        private List<DropItemData> _drops;
        private ItemsFactory _itemsFactory;

        public LootDropOnReviveComponent(List<DropItemData> drops)
        {
            _drops = drops;
        }

        [Inject]
        private void InjectServices(ItemsFactory itemsFactory)
        {
            _itemsFactory = itemsFactory;
        }

        public override void Init()
        {
            _bodyComponent = GetComponent<BodyComponent>();
            _dropHolder = GetComponent<DropHolderComponent>();

            _bodyComponent.OnRevive += OnRevive;
        }

        private void OnRevive()
        {
            _dropHolder.Clear();

            foreach (var drop in _drops)
            {
                if(RandomHelper.Range(0, 100) < drop.Chance)
                {
                    Item item = _itemsFactory.CreateItem(drop.ItemID, count: drop.Amount);
                    _dropHolder.AddItem(item);
                }
            }
        }
    }
}
