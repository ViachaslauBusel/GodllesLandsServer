namespace Game.UnitVisualization
{
    public interface IViewComponent 
    {
        IViewComponent Clone();
        void SetNeedChaceVisual(bool isNeedChaceVisual);
        void SetVisualObjecId(int iD);
    }
}