using Game.Animation;
using Game.Equipment.Components;
using Game.Messenger;
using Game.ObjectInteraction.MiningStone.Commands;
using Game.Physics.Transform;
using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Replicated.Animation;
using Protocol.MSG.Game.Equipment;
using Protocol.MSG.Game.Messenger;
using System.Numerics;

namespace Game.ObjectInteraction.MiningStone
{
    public class CharacterMiningController : Component
    {
        private TransformComponent _transform;
        private ObjectInteractionProcessorComponent _objectInteractionProcessor;
        private AnimatorComponent _animator;
        private MessageBroadcastComponent _messageBroadcast;
        private EquipmentComponent _equipment;
        private long _endMiningTime;
        private Vector3 _startInteractPoint;
        private GameObject _miningStone;
        private bool _isMining;

        public CharacterMiningController(GameObject miningStone)
        {
           _miningStone = miningStone;
        }

        public override void Init()
        {
            _transform = GetComponent<TransformComponent>();
            _objectInteractionProcessor = GetComponent<ObjectInteractionProcessorComponent>();
            _animator = GetComponent<AnimatorComponent>();
            _messageBroadcast = GetComponent<MessageBroadcastComponent>();
            _equipment = GetComponent<EquipmentComponent>();
        }

        public override void Start()
        {
            Debug.Log.Info("CharacterMiningController has been started");
            _endMiningTime = Time.Milliseconds + 5_000;

            if (_equipment.GetItem(EquipmentType.PickaxeTool) == null)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "You need a pickaxe to mine");
                _objectInteractionProcessor.StopInteraction(_miningStone.ID);
                return;
            }
            _startInteractPoint = _transform.Position;
            StartMining();
        }

        private void OnPositionChanged(Vector3 vector)
        {
            float distance = Vector3.Distance(_startInteractPoint, vector);
            if(distance > 1f)
            {
                _objectInteractionProcessor.StopInteraction(_miningStone.ID);
                StopMining();
            }
        }

        private void OnInteractionStarted(int objId)
        {
            if(objId != _miningStone.ID)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "You can't mine two stones at the same time");
                StopMining();
            }
        }

        private void OnInteractionEnded(int objId)
        {
            if (objId == _miningStone.ID)
            {
                _messageBroadcast.SendMessage(MsgLayer.System, "Mining has been interrupted");
                StopMining();
            }
        }

        public override void Update()
        {
            if (Time.Milliseconds >= _endMiningTime)
            {
                StopMining();
                _miningStone.SendCommand(new MiningCompletionCommand() { CharacterObj = GameObject });
            }
        }

        private void StartMining()
        {
            if (_isMining == true) return;
            _objectInteractionProcessor.OnInteractionStarted += OnInteractionStarted;
            _objectInteractionProcessor.OnInteractionEnded += OnInteractionEnded;
            _transform.OnPositionChanged += OnPositionChanged;
            _animator.SetState(AnimationStateID.Mining, true);
            _isMining = true;
        }

        private void StopMining()
        {
            if (_isMining == false) return;
            _objectInteractionProcessor.OnInteractionStarted -= OnInteractionStarted;
            _objectInteractionProcessor.OnInteractionEnded -= OnInteractionEnded;
            _transform.OnPositionChanged -= OnPositionChanged;
            _animator.SetState(AnimationStateID.Mining, false);
            _isMining = false;
        }

        public override void OnDestroy()
        {
            StopMining();
            Debug.Log.Info("CharacterMiningController has been destroyed");
        }
    }
}
