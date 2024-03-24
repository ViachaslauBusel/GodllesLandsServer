using Game.Animation;
using NetworkGameEngine;
using Protocol.Data.Replicated.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Units.Corpse
{
    public class CorpseControllerComponent : Component
    {
        private AnimatorComponent _animator;

        public override void Init()
        {
            _animator = GetComponent<AnimatorComponent>();
        }

        public override void Start()
        {
            _animator.SetState(AnimationStateID.Dead, true);
        }
    }
}
