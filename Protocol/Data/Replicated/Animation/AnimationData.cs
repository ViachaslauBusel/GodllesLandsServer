using System.Numerics;

namespace Protocol.Data.Replicated.Animation
{
    [MessageObject]
    public struct AnimationData
    {
        public AnimationID AnimationID { get; set; }
        public AnimationLayer AnimationLayer { get; set; }
        public int PlaybackTime { get; set; }
        public Vector3? Direction { get; set; }
    }
}