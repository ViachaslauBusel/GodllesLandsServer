using Game.Drop;
using Game.Inventory.Components;
using Game.Items;
using Game.Items.Components;
using Game.Physics.Transform;
using Game.Systems.Stats.Components;
using Game.Tools;
using Godless_Lands_Game.GameObjectFactory;
using NetworkGameEngine;
using NetworkGameEngine.JobsSystem;
using Protocol.Data.Replicated.Skins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.Drop
{
    public class InventoryDropOnDeathComponent : Component
    {
        private InventoryComponent _inventory;
        private BodyComponent _bodyComponent;
        private ItemStorageComponent _itemStorage;
        private TransformComponent _transform;

        public override void Init()
        {
            _inventory = GetComponent<InventoryComponent>();
            _bodyComponent = GetComponent<BodyComponent>();
            _itemStorage = GetComponent<ItemStorageComponent>();
            _transform = GetComponent<TransformComponent>();

            _bodyComponent.OnDeath += OnDeath;
        }

        private async void OnDeath()
        {
            var items = _inventory.SecondaryInventory.TakeAllItems();
            List<Job> jobs = new List<Job>();
            foreach (var item in items)
            {
                jobs.Add(_itemStorage.DestroyItem(item));
            }

            await Job.WhenAll(jobs);

            var dropHolder = new DropHolderComponent();
            dropHolder.AddItems(items);

            Vector3 randomDirection = new Vector3(RandomHelper.Range(-1f, 1f), 0, RandomHelper.Range(-1f, 1f));
            randomDirection = Vector3.Normalize(randomDirection);
            GameObject dropBag = DropBagFactory.CreateDropBag(_transform.Position + randomDirection, DropBagType.Large, dropHolder);
            _ = GameObject.World.AddGameObject(dropBag);
        }
    }
}
