using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AI
{
    internal abstract class BaseState
    {
        private AiControllerComponent _owner;

        public BaseState(AiControllerComponent component)
        {
            _owner = component;
        }

        public virtual void Active() { }
        public virtual void Deactive() { }

        public abstract bool Update(out AiState newState);
    }
}
