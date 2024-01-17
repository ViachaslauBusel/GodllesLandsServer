using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using Protocol.Data.Replicated.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Animation
{
    public class AnimatorComponent : Component, IReadData<AnimationPlaybackBuffer>, IReadData<AnimationStateData>
    {
        private List<AnimationData> _dumpForSync = new List<AnimationData>();
        private List<AnimationData> _events = new List<AnimationData>();
        private AnimationStateData _activeState;

        public override void Init()
        {
            _activeState = new AnimationStateData()
            {
                AnimationStateID = AnimationStateID.None,
                Version = 0
            };
        }


        public void Play(AnimationID id, AnimationLayer layer) => Play(id, layer, 0, null);
        public void Play(AnimationID id, AnimationLayer layer, int playbackTime) => Play(id, layer, playbackTime, null);
        public void Play(AnimationID id, AnimationLayer layer, int playbackTime, Vector3? direction)
        {
            AnimationData animationData = new AnimationData();
            animationData.AnimationID = id;
            animationData.AnimationLayer = layer;
            animationData.PlaybackTime = playbackTime;
            animationData.Direction = direction;
            Play(animationData);
        }

        public void Play(AnimationData animation)
        {
            _events.Add(animation);
        }

        public void SetState(AnimationStateID id, bool active)
        {
            // Deactivate state
            if(active == false)
            {
                if(_activeState.AnimationStateID == id)
                {
                    _activeState.AnimationStateID = AnimationStateID.None;
                    _activeState.Version++;
                }
                else Debug.Log.Warn($"AnimatorComponent: Can't deactivate state {id} because it's not active");
            }
            else // Activate state
            {
                if(_activeState.AnimationStateID != 0)
                {
                    Debug.Log.Debug($"This state {_activeState.AnimationStateID} was deactivated by state {id}");
                }
                _activeState.AnimationStateID = id;
                _activeState.Version++;
            }
        }

        public void UpdateData(ref AnimationPlaybackBuffer data)
        {
            _dumpForSync.Clear();

            if (_events.Count > 0)
            {
                _dumpForSync.AddRange(_events);
                _events.Clear();

                data.PlaybackHistory = _dumpForSync;
                data.Version++;
            }
        }

        public void UpdateData(ref AnimationStateData data)
        {
           data = _activeState;
        }
    }
}
