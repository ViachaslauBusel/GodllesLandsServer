using Game.NetworkTransmission;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.Hotbar;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Hotbar
{
    internal class HotbarComponent : Component
    {
        private NetworkTransmissionComponent _networkTransmission;
        private List<HotbarCellHolder> _cells = new List<HotbarCellHolder>();
        private List<HotbarCell> _syncDump = new List<HotbarCell>();

        public override void Start()
        {
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _networkTransmission.RegisterHandler(Opcode.MSG_HOTBAR_SET_CELL_VALUE, HotbarSetCellValue);
        }

        private void HotbarSetCellValue(Packet packet)
        {
            packet.Read(out MSG_HOTBAR_SET_CELL_VALUE_CS msg);

            HotbarCellHolder cell = GetOrCreateCell(msg.CellIndex);

            cell.SetValue(msg.CellType, msg.CellValue);

            SyncWithClient();
        }

        private void SyncWithClient()
        {
            _syncDump.Clear();
           
            foreach (HotbarCellHolder cell in _cells)
            {
                if (cell.IsDirty)
                {
                    _syncDump.Add(cell.GetCell());
                    cell.MarkAsClean();
                }
            }

            MSG_HOTBAR_UPDATE_SC msg = new MSG_HOTBAR_UPDATE_SC();
            msg.Cells = _syncDump;
            _networkTransmission.Socket.Send(msg);
        }

        private HotbarCellHolder GetOrCreateCell(byte cellIndex)
        {
            HotbarCellHolder cell = _cells.FirstOrDefault(c => c.CellIndex == cellIndex);

            if(cell == null)
            {
                cell = new HotbarCellHolder(cellIndex);
                _cells.Add(cell);
            }

            return cell;
        }

        public override void OnDestroy()
        {
            _networkTransmission.UnregisterHandler(Opcode.MSG_HOTBAR_SET_CELL_VALUE);
        }
    }
}
