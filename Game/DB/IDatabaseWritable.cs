using NetworkGameEngine.JobsSystem;

namespace Game.DB
{
    public interface IDatabaseWritable
    {
        DatabaseSavePriority DatabaseSavePriority { get; }
        bool HasDataToSave { get; set; }

        Job<bool> WriteToDatabase();
    }
}
