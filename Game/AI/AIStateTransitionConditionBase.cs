using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AI
{
    public abstract class AIStateTransitionConditionBase
    {
        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }
        public abstract bool CheckCondition(out AiState newState);
    }
}
