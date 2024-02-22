using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using NetworkGameEngine.JobsSystem;

namespace Game.DB
{
    public class DBControlComponent : Component
    {
        struct WriteJob
        {
            public IDatabaseWritable Writable;
            public Job<bool> Job;
        }

        //private List<IDatabaseWritable> _databaseWritables;
        private Dictionary<DatabaseSavePriority, List<IDatabaseWritable>> _databaseWritablesByPriority = new();
        private List<IDatabaseReadable> _databaseReadables;
        private bool _isAllDataReaded = false;
        private List<WriteJob> _writeJobs = new List<WriteJob>();
        private uint _tickCounter = 0;

        public event Action OnDatabaseLoadComplete;

        public bool IsAllDataReaded => _isAllDataReaded;

        public override void Start()
        {
            // Register all database writables.
            var databaseWritables = GetComponents<IDatabaseWritable>();
            foreach (var writable in databaseWritables)
            {
                if (!_databaseWritablesByPriority.ContainsKey(writable.DatabaseSavePriority))
                {
                    _databaseWritablesByPriority.Add(writable.DatabaseSavePriority, new List<IDatabaseWritable>());
                }

                _databaseWritablesByPriority[writable.DatabaseSavePriority].Add(writable);
            }
            // Register all database readables. 
            _databaseReadables = GetComponents<IDatabaseReadable>();

            enabled = false;
        }

        internal void StartLoadFromDatabase()
        {
            ReadAllDataFromDatabase();
        }

        private async void ReadAllDataFromDatabase()
        {
            List<Job> readJobs = new List<Job>();

            foreach (var readable in _databaseReadables)
            {
                readJobs.Add(readable.ReadFromDatabase());
            }

            await Job.WhenAll(readJobs);

            _isAllDataReaded = enabled = true;
            OnDatabaseLoadComplete?.Invoke();
        }

        public override void Update()
        {
            _tickCounter++;

            ProcessDatabaseWritables(DatabaseSavePriority.SuperHigh, 10);
            ProcessDatabaseWritables(DatabaseSavePriority.Hight, 600);
            ProcessDatabaseWritables(DatabaseSavePriority.Medium, 6_000);

            if (_writeJobs.Count > 0)
            {
                _ = AwaitSaveDataToDatabase();
            }
        }

        private void ProcessDatabaseWritables(DatabaseSavePriority priority, uint tickCondition)
        {
            if (_databaseWritablesByPriority.ContainsKey(priority) && (_tickCounter % tickCondition == 0))
            {
                foreach (var writable in _databaseWritablesByPriority[priority])
                {
                    if (writable.HasDataToSave)
                    {
                        _writeJobs.Add(new WriteJob()
                        {
                            Writable = writable,
                            Job = writable.WriteToDatabase()
                        });
                        writable.HasDataToSave = false;
                    }
                }
            }
        }

        private async Job AwaitSaveDataToDatabase()
        {
            // Disable the component until all write tasks are completed.
            enabled = false;
            await Job.WhenAll(_writeJobs.Select(j => j.Job));

            foreach (var job in _writeJobs)
            {
                if (job.Job.IsFaulted || job.Job.GetResult() == false)
                {
                    Debug.Log.Error($"[{job.Writable}] Failed to write data to database.");
                    job.Writable.HasDataToSave = true;
                }
            }

            enabled = true;
        }

        internal async Job<bool> Shootdown()
        {
            Debug.Log.Debug($"DBControlComponent Shootdown");
            ProcessDatabaseWritables(DatabaseSavePriority.SuperHigh, 1);
            ProcessDatabaseWritables(DatabaseSavePriority.Hight, 1);
            ProcessDatabaseWritables(DatabaseSavePriority.Medium, 1);
            ProcessDatabaseWritables(DatabaseSavePriority.Low, 1);

            Debug.Log.Debug($"DBControlComponent Shootdown ProcessDatabaseWritables:{_writeJobs.Count}");
            if (_writeJobs.Count > 0)
            {
               await AwaitSaveDataToDatabase();
               
                Debug.Log.Debug($"DBControlComponent Shootdown SaveDataToDatabase");
                return true;
            }
            return false;
        }
    }
}
