using Game.Systems.Stats;
using Game.Systems.Target;
using NetworkGameEngine;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AI.StateTransitionConditions
{
    internal class ToAttackStateCondition : AIStateTransitionConditionBase
    {
        private DamageReceiverComponent _damageReceiver;
        private TargetManagerComponent _targetManager;

        public ToAttackStateCondition(AiControllerComponent aiController)
        {
            _damageReceiver = aiController.GetComponent<DamageReceiverComponent>();
            _targetManager = aiController.GetComponent<TargetManagerComponent>();
        }

        public override void OnActivate()
        {
            _damageReceiver.OnDamageReceiving += OnDamageReceive;
        }

        private void OnDamageReceive(GameObject attacker, int damage)
        {
            if (attacker.IsDestroyed == false) 
            {
                _targetManager.SetTarget(attacker);
            }
        }

        public override bool CheckCondition(out AiState newState)
        {
            newState = AiState.Attack;
            return _targetManager.Target != null;
        }

        public override void OnDeactivate()
        {
            _damageReceiver.OnDamageReceiving -= OnDamageReceive;
        }
    }
}
