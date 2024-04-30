namespace Game.Physics
{
    [Flags]
    public enum LayerMask : ushort
    {
        ALL = ushort.MaxValue,
        Default = 0,
        Ground = 1,
        StaticObject = 2
    }
}