using Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;

namespace Godless_Lands_Game.UnitVisualization
{
    public class DropBagViewComponent : BaseViewComponent, IReadData<DropBagSkinData>
    {
        private DropBagType _bagType;

        public DropBagViewComponent(DropBagType bagType)
        {
            _bagType = bagType;
        }

        public void UpdateData(ref DropBagSkinData data)
        {
            data.BagType = _bagType;
            data.Version = _version;
        }
    }
}
