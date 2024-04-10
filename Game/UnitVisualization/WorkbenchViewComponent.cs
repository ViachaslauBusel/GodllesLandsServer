using Game.UnitVisualization;
using NetworkGameEngine;
using Protocol.Data.Replicated.Skins;
using Protocol.Data.Workbenches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.UnitVisualization
{
    public class WorkbenchViewComponent : BaseViewComponent, IReadData<WorkbenchSkinData>
    {
        private WorkbenchType _workbenchType;

        public WorkbenchType WorkbenchType => _workbenchType;

        public WorkbenchViewComponent(WorkbenchType workbenchType)
        {
            _workbenchType = workbenchType;
        }

        public void UpdateData(ref WorkbenchSkinData data)
        {
            data.WorkbenchType = _workbenchType;
            data.Version = _version;
        }
    }
}
