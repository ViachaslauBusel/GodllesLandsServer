using Database;
using Game.DB;
using Game.NetworkTransmission;
using NetworkGameEngine;
using NetworkGameEngine.ContinuationTaskExecution;
using NetworkGameEngine.JobsManagment;
using NetworkGameEngine.Units.Characters;
using Protocol;
using Protocol.MSG.Game.Hotbar;
using RUCP;

namespace Game.Hotbar
{
    internal class HotbarComponent : Component, IDatabaseReadable, IDatabaseWritable
    {
        private NetworkTransmissionComponent _networkTransmission;
        private CharacterInfoHolder _characterInfoHolder;
        private List<HotbarCellHolder> _cells = new List<HotbarCellHolder>();
        private List<HotbarCell> _syncDump = new List<HotbarCell>();

        public DatabaseSavePriority DatabaseSavePriority => DatabaseSavePriority.SuperHigh;

        public bool HasDataToSave { get; set; }

        public override async void Init()
        {
            _characterInfoHolder = GetComponent<CharacterInfoHolder>();
            _networkTransmission = GetComponent<NetworkTransmissionComponent>();
            _networkTransmission.RegisterHandler(Opcode.MSG_HOTBAR_SET_CELL_VALUE, HotbarSetCellValue);
        }

        public async Job ReadFromDatabase()
        {
            HotbarCell[] cells = await JobsSystem.Execute(GameDatabaseProvider.Select<HotbarCell[]>($"SELECT get_hotbar('{_characterInfoHolder.CharacterID}')"));
            if (cells != null)
            {
                foreach (HotbarCell cell in cells)
                {
                    GetOrCreateCell(cell.CellIndex).SetValue(cell.CellType, cell.CellValue);
                }
            }

            SyncWithClient();
        }

        public async Job<bool> WriteToDatabase()
        {
            JobContainer<bool> container = new JobContainer<bool>();
            foreach(var cell in _cells)
            {
                if (cell.DatabaseSyncRequired)
                {
                    container.AddTask(GameDatabaseProvider.Call($"CALL set_hotbar({_characterInfoHolder.CharacterID}, {cell.CellIndex}, {(int)cell.CellType}, {cell.CellValue})"));
                    cell.ResetDatabaseSyncFlag();
                }
            }

            bool[] results = await container.WaitAll();

            return results.All(result => result);
        }

        private void HotbarSetCellValue(Packet packet)
        {
            packet.Read(out MSG_HOTBAR_SET_CELL_VALUE_CS msg);

            HotbarCellHolder cell = GetOrCreateCell(msg.CellIndex);

            cell.SetValue(msg.CellType, msg.CellValue);

            SyncWithClient();
            HasDataToSave = true;
        }

        private void SyncWithClient()
        {
            _syncDump.Clear();

            foreach (HotbarCellHolder cell in _cells)
            {
                if (cell.ClientSyncRequired)
                {

                    _syncDump.Add(cell.GetCell());
                    cell.ResetClientSyncFlag();
                }
            }

            if (_syncDump.Count > 0)
            {
                MSG_HOTBAR_UPDATE_SC msg = new MSG_HOTBAR_UPDATE_SC();
                msg.Cells = _syncDump;
                _networkTransmission.Socket.Send(msg);
            }
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
