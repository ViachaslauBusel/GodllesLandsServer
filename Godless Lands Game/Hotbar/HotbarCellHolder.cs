using Protocol.MSG.Game.Hotbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Hotbar
{
    public class HotbarCellHolder
    {
        private HotbarCell _cell;
        private bool _clientSyncRequired;
        private bool _databaseSyncRequired;


        public HotbarCellHolder(byte cellIndex)
        {
            _cell = new HotbarCell
            {
                CellIndex = cellIndex,
                CellType = HotbarCellType.Unknown,
                CellValue = 0,
            };
            _clientSyncRequired = true;
        }

        public byte CellIndex => _cell.CellIndex;
        public HotbarCellType CellType => _cell.CellType;
        public short CellValue => _cell.CellValue;
        public bool ClientSyncRequired => _clientSyncRequired;
        public bool DatabaseSyncRequired => _databaseSyncRequired;


        internal void SetValue(HotbarCellType cellType, short cellValue)
        {
            _cell.CellType = cellType;
            _cell.CellValue = cellValue;
            _clientSyncRequired = true;
            _databaseSyncRequired = true;
        }

        public void ResetClientSyncFlag()
        {
            _clientSyncRequired = false;
        }

        public void ResetDatabaseSyncFlag()
        {
            _databaseSyncRequired = false;
        }


        internal HotbarCell GetCell() => _cell;
    }
}
