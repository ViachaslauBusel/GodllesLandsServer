using NetworkGameEngine;
using Protocol.Data.Replicated.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.Animation
{
    public class AnimatorComponent : Component, IReadData<AnimationPlaybackBuffer>
    {
        private List<AnimationData> _dumpForSync = new List<AnimationData>();
        private List<AnimationData> _events = new List<AnimationData>();


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
    }
}
