using NetworkGameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AI
{
    public abstract class BaseState
    {
        protected AiControllerComponent _owner;
        private List<AIStateTransitionConditionBase> _conditions;

        public BaseState(AiControllerComponent component)
        {
            _owner = component;
            _conditions = new List<AIStateTransitionConditionBase>();
        }

        public BaseState AttachCondition(AIStateTransitionConditionBase condition)
        {
            _conditions.Add(condition);
            return this;
        }

        public void Activate()
        {
            foreach(var c in _conditions) { c.OnActivate();}
            OnActive();
        }

        public void Deactivate()
        {
            foreach(var c in _conditions) { c.OnDeactivate(); }
            OnDeactive();
        }

        public virtual void OnActive() { }
        public virtual void OnDeactive() { }
        public abstract bool Update(out AiState newState);

        public bool IsStateChangeRequired(out AiState newState)
        {
            foreach(var c in _conditions) 
            {
                if(c.CheckCondition(out newState)) return true;
            }

            return Update(out newState);
        }
    }
}
