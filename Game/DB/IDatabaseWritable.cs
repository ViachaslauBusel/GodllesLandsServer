using NetworkGameEngine.JobsSystem;

namespace Game.DB
{
    public interface IDatabaseWritable
    {
        DatabaseSavePriority DatabaseSavePriority { get; }
        bool HasDataToSave { get; }

        Job<bool> WriteToDatabase();
    }
}
