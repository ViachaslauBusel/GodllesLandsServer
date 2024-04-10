using DataFileProtocol.Skills;
using Game.Animation;
using Game.Physics.Transform;
using Game.RespawnPoints;
using NetworkGameEngine;
using Protocol.Data.Replicated.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.Skills.Handler
{
    internal class TeleportSkillHandler : ISkillHandler
    {
        private TransformComponent _transform;
        private RespawnPointsService _respawnPointsService;
        private AnimatorComponent _animator;
        private SkillData _data;
        private bool _inUse = false;

        public bool InUse => _inUse;

        [Inject]
        private void InjectServices(RespawnPointsService respawnPointsService)
        {
            _respawnPointsService = respawnPointsService;
        }

        public void Init(Component component, SkillData skill)
        {
            _transform = component.GetComponent<TransformComponent>();
            _animator = component.GetComponent<AnimatorComponent>();
            _data = skill;
        }

        public bool PreProcessSkill(GameObject target)
        {
         
            _animator.Play((AnimationID)_data.animationId, AnimationLayer.TimeAnimation, _data.applyingTime);
            _inUse = true;
            return true;
        }

        public void ApplySkill()
        {
        }

        public void PostProcessSkill()
        {
            _inUse = false;
            _transform.TeleportTo(_respawnPointsService.GetNeareatPoint(_transform.Position));
        }

       
    }
}
