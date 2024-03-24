using Game.Drop;
using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Corpse
{
    public class TimedDestructionComponent : Component
    {
        private const long DROP_LIFETIME = 40_000;
        private const long NO_DROP_LIFETIME = 15_000;
        private const long LIFETIME_AFTER_TAKE_DROP = 7_000;

        private bool _isDropListEmpty;
        private long _destroyTime;
        private DropHolderComponent _dropHolder;

        public override void Init()
        {
            _dropHolder = GetComponent<DropHolderComponent>();

            _isDropListEmpty = _dropHolder.DropList.Count == 0;
            _destroyTime = Time.Milliseconds + (_isDropListEmpty ? NO_DROP_LIFETIME : DROP_LIFETIME);

            _dropHolder.OnUpdateDropList += OnUpdateDropList;
        }

        private void OnUpdateDropList()
        {
            if(_isDropListEmpty == false && _dropHolder.DropList.Count == 0)
            {
                _isDropListEmpty = true;
                _destroyTime = Time.Milliseconds + LIFETIME_AFTER_TAKE_DROP;
            }
        }

        public override void Update()
        {
            if (_destroyTime <= Time.Milliseconds)
            {
                GameObject.World.RemoveGameObject(GameObject.ID);
            }
        }
    }
}
